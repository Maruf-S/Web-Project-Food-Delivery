using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Models
{
    public class Employee : ApplicationUser
    {
    public string GeoLocation { get; set; }
}
}
