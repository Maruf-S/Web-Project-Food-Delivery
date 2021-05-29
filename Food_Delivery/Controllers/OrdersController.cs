using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Controllers
{
    public class OrdersController : Controller
    {
        [HttpGet]
        public IActionResult Cart()
        {
            return View();
        }
    }
}
