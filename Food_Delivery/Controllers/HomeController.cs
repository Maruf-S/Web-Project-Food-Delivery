using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Food_Delivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Food_Delivery.Helpers;

namespace Food_Delivery.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var rnd = new Random();
            ViewBag.Fres = _context.Restaurants
                .ToList()
                .OrderBy(x => rnd.Next())
                .Take(4)
                ;
            ViewBag.FFood = _context.Foods
                .ToList()
            .OrderBy(x => rnd.Next())
            .Take(4);

            return View();
        }
        public IActionResult About() {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        /*[HttpGet]
        public IActionResult RestourantDetail(string query)
        {
            if (query == null)
            {
                return View(_context.Restaurants.ToList());
            }
            ViewBag.query = query;
            return View(_context.Restaurants.Where(r => r.Name.StartsWith(query)).ToList());
        }*/


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
