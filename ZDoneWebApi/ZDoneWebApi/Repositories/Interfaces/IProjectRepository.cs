using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> Get(int id);

        Task<Project> GetByUserId(string id);

        Task Create(Project project);

        Task Update(Project project);

        Task Delete(int id);
    }
}