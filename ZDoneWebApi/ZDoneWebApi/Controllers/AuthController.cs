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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (model == null)
                return BadRequest("Invalid client request");

            var user = await _accountBl.GetUserByName(model.UserName);
            if (user == null)
            {
                return BadRequest("User not found!");
            }
            if (!await _accountBl.CheckPassword(user, model.Password))
            {
                return Unauthorized("Wrong password");
            }
            var userClaims = await _authBl.GetClaimsAsync(user);

            var tokenString = _authBl.GetAccessToken(userClaims);

            return Ok(new { Token = tokenString });
        }
    }
}