using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IListBl
    {
        Task<IEnumerable<ListDto>> GetAllAsync();

        Task<IEnumerable<ListDto>> GetAllByFolderID(int id);

        Task<ListDto> ReadAsync(int id);

        Task<ItemResponse> CreateAsync(ListDto list);

        Task<ItemResponse> UpdateAsync(ListDto list);

        Task<ItemResponse> DeleteAsync(int id);
    }
}