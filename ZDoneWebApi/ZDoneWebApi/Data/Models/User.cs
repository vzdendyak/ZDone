using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDoneWebApi.Data.Models
{
    [Table("Users")]
    public class User
    {
        #region Main props

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        #endregion Main props

        #region Navigation props

        public virtual ICollection<ProjectsUsers> ProjectsUsers { get; set; }

        #endregion Navigation props
    }
}