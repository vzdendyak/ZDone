using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDoneWebApi.Data.Models
{
    [Table("Items")]
    public class Item
    {
        #region Main props

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public int? Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }

        public int ListId { get; set; }
        public int StatusId { get; set; }
        public int? ParentId { get; set; }

        #endregion Main props

        #region Navigation props

        public Status Status { get; set; }
        public List List { get; set; }
        public Item Parent { get; set; }
        public virtual ICollection<Item> Children { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        #endregion Navigation props
    }
}