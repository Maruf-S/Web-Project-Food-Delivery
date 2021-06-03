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


        [Required]
        public int FoodId { get; set; }

        [ForeignKey("FoodId")]
        public virtual Food Food { get; set; }

        //[Required]
        public int Quantity { get; set; }

        public int BatchId { get; set; }

        [ForeignKey("BatchId")]
        [InverseProperty("OrdersList")]
        public virtual OrderBatch OrderBatch { get; set; }
    }
}
