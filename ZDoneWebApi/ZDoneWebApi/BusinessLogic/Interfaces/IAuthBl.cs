using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ZDoneWebApi.Data.DTOs.Auth;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Data.Models.Auth;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IAuthBl
    {
        Task<AuthResultDto> GenerateAuthResultAsync(IdentityUser user);

        Task<AuthResultDto> LoginAsync(LoginModel model);

        Task<AuthResultDto> RegisterAsync(RegisterModel model);

        Task<AuthResultDto> RefreshTokenAsync(string token, string refreshToken);

        Task LogOutAsync(HttpContext context);

        ClaimsPrincipal GetClaimsPrincipalFromToken(string token);

        bool ValidateTokenExpiry(string token);
    }
}