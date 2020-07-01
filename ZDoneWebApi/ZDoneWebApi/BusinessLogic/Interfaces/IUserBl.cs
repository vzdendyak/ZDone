using System.Threading.Tasks;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IUserBl
    {
        Task<bool> IsHaveProjectPermission(string userId, int projectId);

        Task<int> GetBasicListIdId(string userId);

        Task<int> GetBasicFolderId(string userId);

        Task<bool> IsHaveAccesToItem(int id, string userId);

        Task<bool> IsHaveAccessToList(int id, string userId);

        Task<bool> isHaveAccessToFolder(int id, string userId);
    }
}