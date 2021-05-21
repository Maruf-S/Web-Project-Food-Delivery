
using Food_Delivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Areas.SystemAdmin.Controllers
{
    [Area("SystemAdmin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APICALLS
        [HttpGet]
        public async Task<IActionResult> UpdateUserLocationAsync() {
            string location = "";

            var employee = await GetCurrentUserAsync();
            if (employee== null) { return NotFound(); }
            employee.GeoLocation = location;
            await _userManager.UpdateAsync(employee);
            return Content("UPDATE SUCCESSFUL");
        }
        #endregion
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
