using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookWorms.Models
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(200)]
        [Display(Name = "Слика")]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Наслов")]
        public string Title { get; set; }


        [Required]
        [StringLength(100)]
        [Display(Name = "Автор")]
        public string Author { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Жанр")]
        public string Genre { get; set; }


        [Required]
        [Display(Name = "Датум на издавање")]
        [Column(TypeName = "date")]
        public DateTime ReleaseDate { get; set; }


        [Required]
        [StringLength(100)]
        [Display(Name = "Издавачка куќа")]
        public string Publisher { get; set; }

        public string Content { get; set; }
    }
}