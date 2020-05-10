using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDoneWebApi.Data.Models
{
    [Table("Statuses")]
    public class Status
    {
        #region Main props

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion Main props

        #region Navigation props

        public virtual ICollection<Item> Items { get; set; }

        #endregion Navigation props
    }
}