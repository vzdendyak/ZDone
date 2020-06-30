using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.BusinessLogic
{
    public class FolderBl : IFolderBl
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IMapper _mapper;
        private readonly IUserBl _userBl;

        public FolderBl(IFolderRepository folderRepository, IMapper mapper, IUserBl userBl)
        {
            _mapper = mapper;
            _folderRepository = folderRepository;
            _userBl = userBl;
        }

        public async Task<IEnumerable<FolderDto>> GetAllAsync()
        {
            var folders = await _folderRepository.ReadAll();

            IEnumerable<FolderDto> dtoItems = _mapper.Map<IEnumerable<Folder>, IEnumerable<FolderDto>>(folders);

            return dtoItems;
        }

        public async Task<IEnumerable<ListDto>> GetRelatedLists(int id, string userId)
        {
            if (!(await _userBl.isHaveAccessToFolder(id, userId)))
            {
                return null;
            }
            var lists = await _folderRepository.GetAllLists(id);
            IEnumerable<ListDto> dtoLists = _mapper.Map<IEnumerable<List>, IEnumerable<ListDto>>(lists);
            return dtoLists;
        }

        public async Task<FolderDto> ReadAsync(int id)
        {
            var folder = await _folderRepository.Read(id);
            var dtoItem = _mapper.Map<FolderDto>(folder);
            var item = _folderRepository.GetLastItemStored();
            return dtoItem;
        }

        public async Task<ItemResponse> CreateAsync(FolderDto folder)
        {
            var origFolder = _mapper.Map<Folder>(folder);
            await _folderRepository.Create(origFolder);

            var returnFolder = _mapper.Map<FolderDto>(_folderRepository.GetLastItemStored());
            return new ItemResponse(true, "Created", returnFolder.Id);
        }

        public async Task<ItemResponse> UpdateAsync(FolderDto folder)
        {
            var existingFolder = await _folderRepository.Read(folder.Id);
            // Get role
            var newFolder = _mapper.Map<Folder>(folder);
            await _folderRepository.Update(newFolder);
            return new ItemResponse(true, "Updated successfully!");
        }

        public async Task<ItemResponse> DeleteAsync(int id)
        {
            var realItem = await _folderRepository.Read(id);

            if (realItem == null) throw new NotImplementedException();
            await _folderRepository.Delete(id);
            return new ItemResponse(true, "Deleted successfully");
        }

        public async Task<IEnumerable<FolderDto>> GetByProjectIdAsync(int id, string userId)
        {
            if (!(await _userBl.IsHaveProjectPermission(userId, id)))
            {
                throw new Exception("You don't have permission to see this project");
            }
            var folders = await _folderRepository.GetByProjectId(id);

            IEnumerable<FolderDto> dtoItems = _mapper.Map<IEnumerable<Folder>, IEnumerable<FolderDto>>(folders);

            return dtoItems;
        }
    }
}