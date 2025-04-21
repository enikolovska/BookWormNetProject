using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Data.Entity.Migrations.Model.UpdateDatabaseOperation;

namespace BookWorms.Models
{
    public class FutureBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string ImageUrl { get; set; }

       
        public int BookId { get; set; }
        public Book Book { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }


}