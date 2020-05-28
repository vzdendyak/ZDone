using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IAccountBl
    {
        Task<bool> CheckPassword(User user, string password);

        Task<User> GetUserByName(string userName);
    }
}