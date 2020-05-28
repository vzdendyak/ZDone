using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDoneWebApi.Data.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        #region Main props

        private string Info { get; set; }

        #endregion Main props

        #region Navigation props

        public virtual ICollection<ProjectsUsers> ProjectsUsers { get; set; }

        #endregion Navigation props
    }
}