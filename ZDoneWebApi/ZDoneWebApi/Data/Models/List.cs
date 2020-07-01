using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDoneWebApi.Data.Models
{
    [Table("Lists")]
    public class List
    {
        #region Main props

        public int Id { get; set; }
        public string Name { get; set; }
        public int FolderId { get; set; }
        public bool IsBasic { get; set; }

        #endregion Main props

        #region Navigation props

        public Folder Folder { get; set; }
        public virtual ICollection<Item> Items { get; set; }

        #endregion Navigation props
    }
}