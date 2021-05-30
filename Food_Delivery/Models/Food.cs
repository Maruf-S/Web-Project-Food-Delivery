using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Food Name :")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Food Description :")]
        [Required]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        [DisplayName("Price :")]
        [Required]
        public double Price { get; set; }

        public double Weight { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        [DisplayName("Food Restaurant Name :")]
        [Required]
        public int ResturantId { get; set; }

        [ForeignKey("ResturantId")]
        public virtual Restaurant Resturant { get; set; }

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
