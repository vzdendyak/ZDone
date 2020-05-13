using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZDoneWebApi.Data.DTOs
{
    public class FolderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
    }
}