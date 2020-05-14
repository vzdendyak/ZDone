using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Repositories.Interfaces
{
    public interface IListRepository
    {
        Task<IEnumerable<List>> ReadAll();

        Task<List> Read(int id);

        Task Create(List list);

        Task Update(List list);

        Task Delete(int id);

        List GetLastItemStored();

        Task<IEnumerable<List>> GetsListsByFolderId(int id);
    }
}