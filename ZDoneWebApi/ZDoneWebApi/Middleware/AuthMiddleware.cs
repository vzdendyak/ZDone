using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.DTOs.Auth;

namespace ZDoneWebApi.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private IAuthBl _authBl;

        public AuthMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //_authBl = context.RequestServices.GetService(typeof(IAuthBl)) as AuthBl;
            _authBl = context.RequestServices.GetService<IAuthBl>();

            AuthResultDto refreshResult = new AuthResultDto();
            var token = context.Request.Cookies[".AspNetCore.Application.Id"];
            if (!string.IsNullOrEmpty(token) && !IsExpiredAccess(token) && _authBl != null)
            {
                var refreshToken = context.Request.Cookies[".AspNetCore.Application.Id-refresh"];
                if (!string.IsNullOrEmpty(token))
                {
                    refreshResult = await _authBl.RefreshTokenAsync(token, refreshToken);
                }
            }
            if (refreshResult.Success)
            {
                context.Request.Headers.Add("Authorization", "Bearer " + refreshResult.Token);
                context.Response.Cookies.Append(".AspNetCore.Application.Id", refreshResult.Token,
                    new CookieOptions
                    {
                    });
                context.Response.Cookies.Append(".AspNetCore.Application.Id-refresh", refreshResult.RefreshToken,
                    new CookieOptions
                    {
                        MaxAge = TimeSpan.FromHours(48)
                    });
            }
            else
            {
                if (!string.IsNullOrEmpty(token))
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
            }
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Xss-Protection", "1");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            Console.WriteLine("HANDLED AT: " + DateTime.Now.ToString());
            Console.WriteLine("Request: " + context.Request.Headers["Authorization"]);
            foreach (var cookie in context.Request.Cookies)
            {
                Console.WriteLine(cookie);
            }
            await _next.Invoke(context);
        }

        public bool IsExpiredAccess(string token)
        {
            return _authBl.ValidateTokenExpiry(token);
        }
    }
}