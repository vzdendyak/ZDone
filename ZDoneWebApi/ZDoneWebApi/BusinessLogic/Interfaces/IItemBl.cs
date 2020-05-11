using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IItemBl
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<ItemDto> ReadAsync(int id);

        Task<ItemResponse> CreateAsync(ItemDto item);

        Task<ItemResponse> UpdateAsync(ItemDto item);

        Task<ItemResponse> DeleteAsync(int id);
        
    }
}