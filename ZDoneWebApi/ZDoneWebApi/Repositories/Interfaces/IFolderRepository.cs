using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> ReadAll();

        Task<IEnumerable<Folder>> GetByProjectId(int id);

        Task<Folder> GetBasicFolderByUserId(int projectId, string userId);

        Task<IEnumerable<List>> GetAllLists(int id);

        Task<Folder> Read(int id);

        Task<Folder> Create(Folder folder);

        Task Update(Folder folder);

        Task Delete(int id);

        Folder GetLastItemStored();
    }
}