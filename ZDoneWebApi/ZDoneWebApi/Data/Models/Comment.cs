using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDoneWebApi.Data.Models
{
    [Table("Comments")]
    public class Comment
    {
        #region Main props

        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }

        #endregion Main props

        #region Navigation props

        public Item Item { get; set; }

        public User User { get; set; }

        #endregion Navigation props
    }
}