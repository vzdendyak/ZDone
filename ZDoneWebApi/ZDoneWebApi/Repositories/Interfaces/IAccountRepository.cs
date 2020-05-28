using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<User> GetAsync(string id);

        Task<bool> CheckPassword(User user, string password);

        Task<User> GetUserByUserName(string userName);
    }
}