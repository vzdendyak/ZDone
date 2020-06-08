using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data.DTOs.Auth;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Data.Models.Auth;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IAccountBl
    {
        Task<bool> CheckPassword(User user, string password);

        Task<User> GetUserByEmail(string userName);
        Task<AuthResultDto> CreateAsync(RegisterModel model);
    }
}