using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly IMapper _mapper;

        public ProjectBl(IProjectRepository projectRepository, IListRepository listRepository, IFolderRepository folderRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _listRepository = listRepository;
            _folderRepository = folderRepository;
            _mapper = mapper;
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