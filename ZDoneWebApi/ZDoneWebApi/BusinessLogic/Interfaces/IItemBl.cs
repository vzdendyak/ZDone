using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IItemBl
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();

        Task<IEnumerable<ItemDto>> GetDateItems(string date);

        Task<IEnumerable<ItemDto>> GetDeletedItems();

        Task<IEnumerable<ItemDto>> GetCompletedItems();

        Task<IEnumerable<ItemDto>> GetUnlistedItems();

        Task<IEnumerable<ItemDto>> GetAllByProject(int id, string userId);

        Task<ItemDto> ReadAsync(int id, string userId, int projectId);

        Task<ItemResponse> CreateAsync(ItemDto item, string userId);

        Task<ItemResponse> UpdateAsync(ItemDto item);

        Task<ItemResponse> DeleteAsync(int id);
    }
}