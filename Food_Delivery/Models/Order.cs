using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime DatePlaced { get; set; }

        public bool Delivered { get; set; }

        [Required]
        public int FoodId { get; set; }

        [ForeignKey("FoodId")]
        public virtual Food Food { get; set; }

        //[Required]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Orders")]
        public virtual ApplicationUser Customer { get; set; }

        //[Required]
        public string EmployeeId { get; set; }

        public int Quantity { get; set; }

        public string DeliveryLoc { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("PendingDeliveries")]
        public virtual ApplicationUser Employee { get; set; }
    }
}
