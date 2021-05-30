using Food_Delivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Controllers
{
    public class Restaurants : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public Restaurants(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index(string query)
        {
            if (query == null)
            {
                return View(_context.Restaurants.ToList());
            }
            ViewBag.query = query;
            return View(_context.Restaurants.Where(r => r.Name.StartsWith(query)).ToList());
        }
        [HttpGet]
        public IActionResult Details(int id) {
            var res = _context.Restaurants
                .Include(i => i.Foods)
                .FirstOrDefault(r => r.Id == id);
            if (res == null) {
                return NotFound();
            }

            return View(res);
        }
        [HttpPost]
        public IActionResult Rate(Rating rate,int id) {
            if (ModelState.IsValid) {
                _context.Ratings.Add(rate);
                _context.SaveChanges();
                return RedirectToAction(actionName: nameof(Details), new { id = id });
            }
            else {
                return RedirectToAction(actionName: nameof(Details), new { id = id });
            }
        }
        public IActionResult Rate() {
            return PartialView("Rating", new Rating());
        }
    }
}