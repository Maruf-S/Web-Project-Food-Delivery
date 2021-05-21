using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Models
{
    public class Resturant
    {
        [Key]
        public int Id { get; set; }
        //[Column("Name")]
        [DisplayName("Resturant Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Rating")]
        public double Rating { get; set; }

        [DisplayName("Adress")]
        public string Address { get; set; }

        public ICollection<Food> Foods { get; set; }

    }
}
