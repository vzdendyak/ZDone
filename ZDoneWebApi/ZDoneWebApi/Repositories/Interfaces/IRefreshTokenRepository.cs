using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models.Auth;

namespace ZDoneWebApi.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken model);

        Task UpdateAsync(RefreshToken model);

        Task DeleteAsync(string key);

        Task<RefreshToken> GetByUserIdAsync(string userId);

        Task<RefreshToken> GetAsync(string key);
    }
}