using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Data.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual UserDto User { get; set; }
        public virtual ICollection<FolderDto> Folders { get; set; }
    }
}