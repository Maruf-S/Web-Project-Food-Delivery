using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Areas.Employee.ViewModels
{
    public class EmployeeUpsertVM
    {

        [Key]
        public string Id { get; set; }
        //[Key]
        //public override string Id { get; set; }
        [Required]
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Required]
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string CVdocumentPath { get; set; }

        public string ImagePath { get; set; }

        //[Required]
        public IFormFile CVDocument { get; set; }
        //[Required]
        public IFormFile Image { get; set; }

    }
}
