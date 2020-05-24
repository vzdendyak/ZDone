using System;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Data.DTOs
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public string Reason { get; set; }

        public int? Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }

        public int? ListId { get; set; }
        public int StatusId { get; set; }
        public int? ParentId { get; set; }
        public ListDto List { get; set; }
    }
}