using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public double Price { get; set; }

        [Required]
        public int ResturantId { get; set; }

        [ForeignKey("ResturantId")]
        public virtual Resturant Resturant { get; set; }

        //[Required]
        //public string CustomerId { get; set; }

        //[ForeignKey("CustomerId")]
        //public virtual ApplicationUser Customer { get; set; }

        //[Required]
        //public string EmployeeId { get; set; }

        //[ForeignKey("EmployeeId")]
        //public virtual ApplicationUser Employee { get; set; }
    }
}
