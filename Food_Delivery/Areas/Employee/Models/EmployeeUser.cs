using Food_Delivery.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Areas.Employee.Models
{
    public class EmployeeUser : ApplicationUser
    {
    public string GeoLocation { get; set; }
    }
}
