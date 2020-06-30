using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.Repositories
{
    public class ListRepository : IListRepository
    {
        private readonly AppDbContext _context;

        public ListRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<List>> ReadAll()
        {
            IEnumerable<List> items = await _context.Lists.ToListAsync();
            return items;
        }

        public async Task<List> Read(int id)
        {
            List list = await _context.Lists.FindAsync(id);

            return list;
        }

        public async Task Create(List list)
        {
            await _context.Lists.AddAsync(list);
            await _context.SaveChangesAsync();
        }

        public async Task Update(List list)
        {
            _context.Entry<List>(list).State = EntityState.Modified;
            _context.Lists.Update(list);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            List list = await _context.Lists.FindAsync(id);
            _context.Lists.Remove(list);
            await _context.SaveChangesAsync();
        }

        public List GetLastItemStored()
        {
            var answer = _context.Lists.FromSqlRaw("select top 1 * from lists order by id desc").ToList();
            return answer[0];
        }

        public async Task<IEnumerable<Item>> GetItemsByListId(int id)
        {
            var lists = await _context.Items.Where(l => l.ListId == id).ToListAsync();
            return lists;
        }

        public async Task<IEnumerable<Item>> GetDoneItemsByListId(int id)
        {
            var lists = await _context.Items.Where(l => l.ListId == id && l.IsDone == true && l.IsDeleted == false).ToListAsync();
            return lists;
        }

        public async Task<IEnumerable<Item>> GetUnDoneItemsByListId(int id)
        {
            var lists = await _context.Items.Where(l => l.ListId == id && l.IsDone == false && l.IsDeleted == false).ToListAsync();
            return lists;
        }
    }
}