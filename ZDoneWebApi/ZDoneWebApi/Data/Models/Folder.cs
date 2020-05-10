using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDoneWebApi.Data.Models
{
    [Table("Folders")]
    public class Folder
    {
        #region Main props

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }

        #endregion Main props

        #region Navigation props

        public Project Project { get; set; }
        public virtual ICollection<List> Lists { get; set; }

        #endregion Navigation props
    }
}