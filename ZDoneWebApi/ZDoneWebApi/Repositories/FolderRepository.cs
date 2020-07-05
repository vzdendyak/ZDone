using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly AppDbContext _context;

        public FolderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Folder>> ReadAll()
        {
            IEnumerable<Folder> folders = await _context.Folders.ToListAsync();
            return folders;
        }

        public async Task<IEnumerable<Folder>> GetByProjectId(int id)
        {
            var folders = await _context.Folders
                .Where(f => f.ProjectId == id)
                .Include(f => f.Lists)
                .ThenInclude(l => l.Items)
                .ToListAsync();
            return folders;
        }

        public async Task<Folder> GetBasicFolderByUserId(int projectId, string userId)
        {
            Folder folder = await _context.Folders.Where(f => f.ProjectId == projectId && f.IsBasic == true).FirstOrDefaultAsync();
            return folder;
        }

        public async Task<Folder> Read(int id)
        {
            Folder folder = await _context.Folders.FindAsync(id);

            return folder;
        }

        public async Task<Folder> Create(Folder folder)
        {
            await _context.Folders.AddAsync(folder);
            await _context.SaveChangesAsync();
            return GetLastItemStored();
        }

        public async Task Update(Folder folder)
        {
            _context.Entry(folder).State = EntityState.Modified;
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Folder folder = await _context.Folders.FindAsync(id);
            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
        }

        public Folder GetLastItemStored()
        {
            var answer = _context.Folders.FromSqlRaw("select top 1 * from folders order by id desc").ToList();
            return answer[0];
        }

        public async Task<IEnumerable<List>> GetAllLists(int id)
        {
            var lists = await _context.Lists.Where(l => l.FolderId == id).Include(i => i.Folder).ToListAsync();
            return lists;
        }
    }
}