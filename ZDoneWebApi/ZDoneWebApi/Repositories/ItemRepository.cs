using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZDoneWebApi.Data;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Item>> ReadAll()
        {
            IEnumerable<Item> items = await _context.Items.ToListAsync();
            return items;
        }

        public async Task<Item> Read(int id)
        {
            Item item = await _context.Items.FindAsync(id);
            return item;
        }

        public async Task Create(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Item item)
        {
             _context.Entry(item).State = EntityState.Modified;
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id )
        {
            Item item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

        }
    }
}
