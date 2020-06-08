using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.DTOs.Auth;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Data.Models.Auth;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.BusinessLogic
{
    public class  AccountBl : IAccountBl
    {
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<User> _userManager;

        public AccountBl(IAccountRepository accountRepository, UserManager<User> userManager)
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            return await _accountRepository.CheckPassword(user, password);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _accountRepository.GetUserByEmail(email);
        }

        public async Task<AuthResultDto> CreateAsync(RegisterModel newUserData)
        {
            var existingUser = _userManager.FindByEmailAsync(newUserData.Email);
            if (existingUser!=null)
            {
                return new AuthResultDto
                {
                    Success = false,
                    ErrorMessages = new[] {"User with this email is already exist!"}
                };
            }
            
            User user = new User { Email = newUserData.Email, UserName = newUserData.Name };
            var result = await _userManager.CreateAsync(user, newUserData.Password);
            if (!result.Succeeded)
                return new AuthResultDto()
                {
                    ErrorMessages = result.Errors.Select(i=>i.Description)
                };
            return new AuthResultDto()
            {
                Success = true
            };

        }
    }
}