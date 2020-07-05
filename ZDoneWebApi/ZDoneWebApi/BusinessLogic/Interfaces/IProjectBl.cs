using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IProjectBl
    {
        Task<int> CheckProjectExist(IdentityUser user);

        Task<ProjectDto> ReadAsync(int id, string userId);

        Task<ProjectDto> CreateAsync(ProjectDto project);

        Task<IEnumerable<ItemDto>> GetDatedItems(string date, string userId);

        Task<IEnumerable<ItemDto>> GetCompletedItems(string userId);

        Task<IEnumerable<ItemDto>> GetDeletedItems(string userId);

        Task<IEnumerable<ItemDto>> GetAllItems(string userId);

        Task<ItemResponse> UpdateAsync(ProjectDto project);

        Task<ItemResponse> DeleteAsync(int id);
    }
}