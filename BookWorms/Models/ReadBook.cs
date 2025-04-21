using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookWorms.Models
{
    public class ReadBook
    {
        public int Id { get; set; }
        public int BookId { get; set; }  
        public Book Book { get; set; }

        [Required]
        public double Rating { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime ReadDate { get; set; }  
        public string Type { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

}