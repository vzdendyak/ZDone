using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.DTOs.Auth;
using ZDoneWebApi.Data.Models.Auth;

namespace ZDoneWebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountBl _accountBl;
        private readonly IAuthBl _authBl;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;

        public AuthController(IAccountBl accountBl, IMapper mapper, IAuthBl authBl)
        {
            _accountBl = accountBl;
            _mapper = mapper;
            _authBl = authBl;
        }

        [HttpGet("isLogined")]
        public IActionResult IsLogined()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)));
            }
            var token = HttpContext.Request.Cookies[".AspNetCore.Application.Id"];

            if (!string.IsNullOrEmpty(token))
                return Ok(true);
            return BadRequest(false);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)));
            }
            if (model == null)
                return BadRequest("Invalid client request");

            var authResponse = await _authBl.LoginAsync(model);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.ErrorMessages);
            }

            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", authResponse.Token,
            new CookieOptions
            {
                //MaxAge = TimeSpan.FromMinutes(2)
                //Expires = DateTime.UtcNow.AddMinutes(2)
            }); ;
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id-refresh", authResponse.RefreshToken,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromHours(48)
            });
            HttpContext.Response.Cookies.Append("User-email", model.Email,
            new CookieOptions
            {
            });
            return Ok(new
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)));
                return BadRequest("Not confirmed bitch!");
            }
            if (!string.Equals(model.ConfirmedPassword, model.Password))
            {
                // return BadRequest("Confirmed password not match");
                return BadRequest(new string[] { "Confirmed password not match" });
            }
            model.Name = model.Name.Trim();
            model.Email = model.Email.Trim();
            var authResult = await _authBl.RegisterAsync(model);
            if (!authResult.Success)
            {
                return BadRequest(authResult.ErrorMessages);
            }
            var cookieOption = new CookieOptions();
            return Ok(new { Message = "Registered" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)));
            }
            if (model == null)
                return BadRequest("Invalid client request");

            var authResponse = await _authBl.RefreshTokenAsync(model.Token, model.RefreshToken);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.ErrorMessages);
            }

            return Ok(new
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)));
            }

            await _authBl.LogOutAsync(HttpContext);
            return Ok();
        }
    }
}