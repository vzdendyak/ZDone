using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IFolderBl
    {
        Task<IEnumerable<FolderDto>> GetAllAsync();

        Task<FolderDto> ReadAsync(int id);

        Task<ItemResponse> CreateAsync(FolderDto folder);

        Task<ItemResponse> UpdateAsync(FolderDto folder);

        Task<ItemResponse> DeleteAsync(int id);
    }
}