using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private UserManager<User> userManager;

        public AccountRepository(UserManager<User> user)
        {
            this.userManager = user;
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<User> GetAsync(string id)
        {
            User applicationUser = await userManager.FindByIdAsync(id);
            if (applicationUser == null)
                throw new KeyNotFoundException();
            else
                return applicationUser;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User applicationUser = await userManager.FindByEmailAsync(email);
            if (applicationUser == null)
                return null;
            else
                return applicationUser;
        }
    }
}