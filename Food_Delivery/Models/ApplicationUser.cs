using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }

        public string GeoLocation { get; set; }

        //nullable
        public string Postion { get; set; }
        
        public ICollection<Order> Orders { get; set; }

        public ICollection<Order> PendingDeliveries { get; set; }
    }
}
