using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string CVdocumentPath { get; set; }

        public string ImagePath { get; set; }
        public ICollection<OrderBatch> Orders { get; set; }

        public ICollection<OrderBatch> PendingDeliveries { get; set; }
    }
}
