using System.Collections.Generic;
using AutoMapper;
using ZDoneWebApi.Data.DTOs;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Data.AutoMapperProfiles
{
    public class AutoMapperProfiler : Profile
    {
        public AutoMapperProfiler()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>();
            CreateMap<Folder, FolderDto>();
            CreateMap<FolderDto, Folder>();
            CreateMap<List, ListDto>();
            CreateMap<ListDto, List>();
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, UserDto>();
        }
    }
}