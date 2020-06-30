using System.Threading.Tasks;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IUserBl
    {
        Task<bool> IsHaveProjectPermission(string userId, int projectId);

        Task<bool> IsHaveAccesToItem(int id, string userId);
        Task<bool> IsHaveAccessToList(int id, string userId);
        Task<bool> isHaveAccessToFolder(int id, string userId);
    }
}