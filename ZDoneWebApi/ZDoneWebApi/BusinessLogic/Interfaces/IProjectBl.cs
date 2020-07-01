using Microsoft.AspNetCore.Identity;
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

        Task<ItemResponse> UpdateAsync(ProjectDto project);

        Task<ItemResponse> DeleteAsync(int id);
    }
}