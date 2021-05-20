using Food_Delivery.Areas.Employee.Models;
using Food_Delivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EmployeeUser> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<EmployeeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var x = GetCurrentUserAsync();
            return View();
        }
        private Task<EmployeeUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
