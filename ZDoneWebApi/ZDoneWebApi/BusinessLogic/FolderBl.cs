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

        public FolderBl(IFolderRepository folderRepository, IMapper mapper)
        {
            _mapper = mapper;
            _folderRepository = folderRepository;
        }

        public async Task<IEnumerable<FolderDto>> GetAllAsync()
        {
            var folders = await _folderRepository.ReadAll();

            IEnumerable<FolderDto> dtoItems = _mapper.Map<IEnumerable<Folder>, IEnumerable<FolderDto>>(folders);

            return dtoItems;
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
    }
}