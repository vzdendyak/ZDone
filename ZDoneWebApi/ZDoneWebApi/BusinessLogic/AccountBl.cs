using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.BusinessLogic
{
    public class AccountBl : IAccountBl
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

        public async Task<User> GetUserByName(string userName)
        {
            return await _accountRepository.GetUserByUserName(userName);
        }
    }
}