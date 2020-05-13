using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> ReadAll();

        Task<Folder> Read(int id);

        Task Create(Folder folder);

        Task Update(Folder folder);

        Task Delete(int id);

        Folder GetLastItemStored();
    }
}