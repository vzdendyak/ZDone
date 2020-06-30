using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(string userId);
    }
}