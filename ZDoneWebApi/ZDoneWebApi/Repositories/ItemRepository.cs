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

        // -
        public async Task<IEnumerable<Item>> GetAll()
        {
            IEnumerable<Item> items = await _context.Items.Include(i => i.List).ToListAsync();
            return items;
        }

        // -
        public async Task<IEnumerable<Item>> GetTodayItems()
        {
            IEnumerable<Item> items = await _context.Items.Where(i => i.ExpiredDate == DateTime.Today && i.IsDeleted == false).Include(i => i.List).ToListAsync();
            return items;
        }

        // -
        public async Task<IEnumerable<Item>> GetDeletedItems()
        {
            IEnumerable<Item> items = await _context.Items.Where(i => i.IsDeleted == true).Include(i => i.List).ToListAsync();
            return items;
        }

        // -
        public async Task<IEnumerable<Item>> GetCompletedItems()
        {
            IEnumerable<Item> items = await _context.Items.Where(i => i.IsDeleted == false && i.IsDone == true).Include(i => i.List).ToListAsync();
            return items;
        }

        // -
        public async Task<IEnumerable<Item>> GetWeekItems()
        {
            IEnumerable<Item> items = await _context.Items.Where(i => i.ExpiredDate >= DateTime.Today && i.ExpiredDate <= DateTime.Today.AddDays(7) && i.IsDeleted == false).Include(i => i.List).ToListAsync();
            return items;
        }

        public async Task<IEnumerable<Item>> GetUnlistedItems()
        {
            IEnumerable<Item> items = await _context.Items.Where(i => i.ListId == null && i.IsDeleted == false).ToListAsync();
            return items;
        }

        public async Task<Item> Read(int id)
        {
            Item item = await _context.Items.Include(i => i.List).FirstOrDefaultAsync(i => i.Id == id);

            return item;
        }

        public async Task Create(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Item item)
        {
            // var item1 = _context.Items.Find(item.Id);
            //item1 = item;
            _context.Entry<Item>(item).State = EntityState.Modified;
            _context.Items.Update(item);
            // _context.Entry<Item>(item).State = EntityState.Detached;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Item item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public Item GetLastItemStored()
        {
            var answer = _context.Items.FromSqlRaw("exec GetId").ToList();
            return answer[0];
        }
    }
}