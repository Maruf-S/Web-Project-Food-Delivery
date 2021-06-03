using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Models
{
    public class OrderBatch
    {
        [Key]
        public int Id { get; set; }
        public string DeliveryLoc { get; set; }

        public DateTime DatePlaced { get; set; }

        public bool Delivered { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Adress { get; set; }
        public string OrderNote { get; set; }
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("PendingDeliveries")]
        public virtual ApplicationUser Employee { get; set; }
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Orders")]
        public virtual ApplicationUser Customer { get; set; }
        public ICollection<Order> OrdersList { get; set; }

    }
}
