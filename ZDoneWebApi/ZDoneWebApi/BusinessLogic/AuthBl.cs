using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.DTOs.Auth;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Data.Models.Auth;
using ZDoneWebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ZDoneWebApi.BusinessLogic
{
    public class AuthBl : IAuthBl
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<User> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthBl(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, IAccountRepository accountRepository, UserManager<User> userManager, TokenValidationParameters tokenValidationParameters)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResultDto> LoginAsync(LoginModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser == null)
                return new AuthResultDto
                {
                    Success = false,
                    ErrorMessages = new[] { "User doesn't exist" }
                };

            var userValidPassword = await _userManager.CheckPasswordAsync(existingUser, model.Password);
            if (!userValidPassword)
                return new AuthResultDto
                {
                    Success = false,
                    ErrorMessages = new[] { "Email/Password are incorrect!" }
                };
            return await GenerateAuthResultAsync(existingUser);
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return new AuthResultDto
                {
                    Success = false,
                    ErrorMessages = new[] { "User with this email is already exist!" }
                };
            }

            User user = new User { Email = model.Email, UserName = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new AuthResultDto()
                {
                    ErrorMessages = result.Errors.Select(i => i.Description)
                };
            return await GenerateAuthResultAsync(user);
        }

        public async Task<AuthResultDto> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetClaimsPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthResultDto { ErrorMessages = new[] { "Invalid token" } };
            }
            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);
            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthResultDto { ErrorMessages = new[] { "This token hasn't expired yet" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenRepository.GetAsync(refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthResultDto { ErrorMessages = new[] { "This refresh token does not exist" } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthResultDto { ErrorMessages = new[] { "This refresh token has expired" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthResultDto { ErrorMessages = new[] { "This refresh token has been invalidated" } };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthResultDto { ErrorMessages = new[] { "This refresh token has been used" } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthResultDto { ErrorMessages = new[] { "This refresh token does not match this JWT" } };
            }

            storedRefreshToken.Used = true;
            await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAuthResultAsync(user);
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                //var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtSecurityToken))
                    return null;

                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<AuthResultDto> GenerateAuthResultAsync(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiryDate = DateTime.UtcNow.AddMinutes(2);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id),
                    new Claim("expires in", expiryDate.Subtract(DateTime.UtcNow).TotalSeconds.ToString())
                }),
                Expires = expiryDate,
                SigningCredentials = credentials,
                Issuer = _configuration["JwtIssuer"],
                Audience = _configuration["JwtAudience"]
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            var newRefreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddHours(48),
                CreationDate = DateTime.UtcNow
            };
            await _refreshTokenRepository.CreateAsync(newRefreshToken);
            return new AuthResultDto()
            {
                Success = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task LogOutAsync(HttpContext context)
        {
            var token = context.Request.Cookies[".AspNetCore.Application.Id"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Response.Cookies.Delete(".AspNetCore.Application.Id");
                context.Response.Cookies.Delete("User-email");
            }
        }
    }
}