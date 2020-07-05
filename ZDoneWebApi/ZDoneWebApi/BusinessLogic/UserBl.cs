using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.BusinessLogic
{
    public class UserBl : IUserBl
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IListRepository _listRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserBl(IUserRepository userRepository, IProjectRepository projectRepository, IItemRepository itemRepository, IFolderRepository folderRepository, IListRepository listRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _listRepository = listRepository;
            _folderRepository = folderRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<int> GetBasicFolderId(string userId)
        {
            var project = await _projectRepository.GetByUserId(userId);
            return project.Folders.Where(f => f.IsBasic == true).FirstOrDefault().Id;
            //return (await _folderRepository.GetBasicFolderByUserId(project.Id, userId)).Id;
        }

        public async Task<int> GetBasicListIdId(string userId)
        {
            var project = await _projectRepository.GetByUserId(userId);
            var listId = project.Folders.Where(f => f.IsBasic == true).FirstOrDefault().Lists.Where(l => l.IsBasic == true).FirstOrDefault().Id;
            return listId;
        }

        public async Task<bool> isHaveAccessToFolder(int id, string userId)
        {
            Folder folder = await _folderRepository.Read(id);
            Project project = await this._projectRepository.Get(folder.ProjectId);
            if (project.UserId != userId)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsHaveAccessToList(int id, string userId)
        {
            List list = await _listRepository.Read(id);
            if (list == null)
            {
                return false;
            }
            Folder folder = await _folderRepository.Read(list.FolderId);
            Project project = await this._projectRepository.Get(folder.ProjectId);
            if (project.UserId != userId)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsHaveAccesToItem(int itemId, string userId)
        {
            Item item = await this._itemRepository.Read(itemId);
            List list = await _listRepository.Read(item.ListId);
            Folder folder = await _folderRepository.Read(list.FolderId);
            Project project = await this._projectRepository.Get(folder.ProjectId);

            if (project.UserId != userId)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsHaveProjectPermission(string userId, int projectId)
        {
            User user = this._userRepository.GetUser(userId);
            Project project = await this._projectRepository.Get(projectId);
            if (project == null)
            {
                return false;
            }
            if (project.UserId == user.Id)
            {
                return true;
            }
            return false;
        }
    }
}