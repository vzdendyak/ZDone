using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IAuthBl
    {
        string GetAccessToken(IEnumerable<Claim> claims);

        Task<Claim[]> GetClaimsAsync(User user);
    }
}