using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Project> GetByUserId(string id)
        {
            var project = await _context.Projects.Where(p => p.UserId == id).Include(p => p.Folders).ThenInclude(p => p.Lists).FirstOrDefaultAsync();
            return project;
        }

        public async Task<Project> Get(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            return project;
        }

        public async Task<Project> Create(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return await GetByUserId(project.UserId);
        }

        public async Task Update(Project project)
        {
            _context.Entry<Project>(project).State = EntityState.Modified;
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Project project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}