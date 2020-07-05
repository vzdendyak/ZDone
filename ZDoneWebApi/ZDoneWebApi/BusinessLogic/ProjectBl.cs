using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.BusinessLogic
{
    public class ProjectBl : IProjectBl
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IListRepository _listRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IUserBl _userBl;
        private readonly IMapper _mapper;

        public ProjectBl(IProjectRepository projectRepository, IListRepository listRepository, IFolderRepository folderRepository, IUserBl userBl, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _listRepository = listRepository;
            _folderRepository = folderRepository;
            _userBl = userBl;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDto>> GetDatedItems(string date, string userId)
        {
            switch (date.ToLower())
            {
                case "today":
                    return await GetItemsByPredicate(userId, (i => i.ExpiredDate == DateTime.Today && i.IsDeleted == false));

                case "week":
                    return await GetItemsByPredicate(userId, i => i.ExpiredDate >= DateTime.Today && i.ExpiredDate <= DateTime.Today.AddDays(7) && i.IsDeleted == false);

                default:
                    return null;
            }
        }

        public async Task<IEnumerable<ItemDto>> GetCompletedItems(string userId)
        {
            return await GetItemsByPredicate(userId, (i => i.IsDeleted == false && i.IsDone == true));
        }

        public async Task<IEnumerable<ItemDto>> GetDeletedItems(string userId)
        {
            return await GetItemsByPredicate(userId, (i => i.IsDeleted == true));
        }

        public async Task<IEnumerable<ItemDto>> GetAllItems(string userId)
        {
            return await GetItemsByPredicate(userId, (i => i.IsDeleted == false));
        }

        public async Task<IEnumerable<ItemDto>> GetItemsByPredicate(string userId, Func<ItemDto, bool> predicate)
        {
            var project = await _projectRepository.GetByUserId(userId);

            if (project == null || project.UserId != userId)
                throw new Exception("Project for this user not found");

            var folders = await _folderRepository.GetByProjectId(project.Id);

            List<ItemDto> itemDtos = new List<ItemDto>();
            foreach (var f in folders)
            {
                foreach (var l in f.Lists)
                {
                    var items = l.Items;
                    var itemsDto = _mapper.Map<IEnumerable<ItemDto>>(items);

                    itemDtos.AddRange(itemsDto.Where(predicate));
                }
            }
            return itemDtos;
        }

        public async Task<int> CheckProjectExist(IdentityUser user)
        {
            var project = await _projectRepository.GetByUserId(user.Id);
            if (project == null)
            {
                await CreateAsync(new ProjectDto { Id = 0, Name = "Basic", UserId = user.Id });
                //await _projectRepository.Create(new Project { Id = 0, Name = "Basic", UserId = user.Id });

                project = await _projectRepository.GetByUserId(user.Id);
                return project.Id;
            }
            return project.Id;
        }

        public async Task<ProjectDto> CreateAsync(ProjectDto project)
        {
            var projectReal = _mapper.Map<Project>(project);
            projectReal = await _projectRepository.Create(projectReal);

            var folder = await _folderRepository.Create(new Folder { Id = 0, Name = "Main", ProjectId = projectReal.Id, IsBasic = true });
            await _listRepository.Create(new List { Id = 0, Name = "Base", FolderId = folder.Id, IsBasic = true });
            project = _mapper.Map<ProjectDto>(projectReal);
            return project;
        }

        public Task<ItemResponse> DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProjectDto> ReadAsync(int id, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ItemResponse> UpdateAsync(ProjectDto project)
        {
            throw new System.NotImplementedException();
        }
    }
}