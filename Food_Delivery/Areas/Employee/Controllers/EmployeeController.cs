using Food_Delivery.Helpers;
using Food_Delivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Food_Delivery.Areas.Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // PUT api/Employee/UpdateLoc
        [HttpPost("UpdateLoc")]
        public async Task<string> UpdateLocAsync([FromHeader] Location location)
        {
            var employee = await GetCurrentUserAsync();
            if (employee == null) { return false.ToString(); }
            employee.GeoLocation = location.ToString();
            await _userManager.UpdateAsync(employee);
            //return location.ToString();
            return true.ToString();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
