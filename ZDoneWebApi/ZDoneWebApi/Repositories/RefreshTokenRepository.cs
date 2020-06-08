using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data;
using ZDoneWebApi.Data.Models.Auth;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(RefreshToken model)
        {
            await _context.RefreshTokens.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string key)
        {
            var model = await GetAsync(key);

            if (model == null)
                return;

            _context.RefreshTokens.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetAsync(string key)
        {
            return await _context.RefreshTokens.FindAsync(key);
        }

        public async Task<RefreshToken> GetByUserIdAsync(string userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(token => token.UserId == userId);
        }

        public async Task UpdateAsync(RefreshToken model)
        {
            _context.RefreshTokens.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}