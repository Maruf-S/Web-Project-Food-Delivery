using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Helpers
{
    public class Location
    {
        [FromHeader]
        public string Longtuide { get; set; }

        [FromHeader]
        public string Latitude { get; set; }

        [FromHeader]
        public string Accuracy { get; set; }
        
        [FromHeader]
        public string Speed { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
