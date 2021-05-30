﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            this.DateAdded = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        //[Column("Name")]
        [DisplayName("Restaurant Name :")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Description :")]
        [Required]
        public string Description { get; set; }

        [DisplayName("Rating :")]
        public int Rating { get; set; }

        [Required]
        [DisplayName("Restaurant Adress :")]
        public string Address { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public string LargeImagePath { get; set; }

        [NotMapped]
        public IFormFile LargeImage { get; set; }

        public DateTime DateAdded { get; set; }
        public ICollection<Food> Foods { get; set; }
        public ICollection<Rating> Ratings { get; set; }


    }
}
//dawit yonas