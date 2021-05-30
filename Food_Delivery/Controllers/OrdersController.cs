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
        [HttpGet]
        public IActionResult CheckOut1()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CheckOut2()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CheckOut3()
        {
            return View();
        }
    }
}
