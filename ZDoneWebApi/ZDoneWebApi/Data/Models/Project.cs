using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDoneWebApi.Data.Models
{
    [Table("Projects")]
    public class Project
    {
        #region Main props

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion Main props

        #region Navigation props

        public virtual ICollection<ProjectsUsers> ProjectsUsers { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }

        #endregion Navigation props
    }
}