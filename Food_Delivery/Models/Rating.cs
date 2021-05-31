using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResturantId { get; set; }

        [ForeignKey("ResturantId")]

        public virtual Restaurant Resturant { get; set; }
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [Required]
        public int RatingN { get; set; }
        [Required]
        public string Review { get; set; }

        public DateTime DateRated { get; set; }


    }
}
