using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        #region APICALLS
        [HttpPost]
        public IActionResult UpdateLocation() {
            return Content("<h1>HELLO</h1>");
        }
        #endregion
    }
}
