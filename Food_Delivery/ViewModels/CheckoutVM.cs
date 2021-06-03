using Food_Delivery.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.ViewModels
{
    public class CheckoutVM
    {
        [Key]
        public int Id { get; set; }

        public DateTime DatePlaced { get; set; }

        public bool Delivered { get; set; }

        [Required]
        public int FoodId { get; set; }

        //[Required]
        public string CustomerId { get; set; }
        //[Required]
        public string EmployeeId { get; set; }

        public int Quantity { get; set; }

        [Required]
        public string DeliveryLoc { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Adress { get; set; }
        public string OrderNote { get; set; }

    }
}
