using Food_Delivery.Models;
using Microsoft.AspNetCore.Authorization;
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
                .Include(i => i.Ratings)
                .Include("Ratings.User")
                .FirstOrDefault(r => r.Id == id);
            if (res == null) {
                return NotFound();
            }

            return View(res);
        }
        [HttpPost]
        //[Authorize]
        public IActionResult Rate([Bind("Review,UserId,ResturantId,RatingN")] Rating rating) {
            if (ModelState.IsValid) {
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (_context.Ratings.Where(r => r.UserId == rating.UserId).FirstOrDefault() != null) {

                    return Redirect(Request.Headers["Referer"].ToString());
                }
                //_context.Ratings.Add(rate);
                rating.DateRated = DateTime.Now;
                _context.Ratings.Add(rating);
                _context.SaveChanges();
                var res = _context.Restaurants
                                    .Include(i => i.Ratings)
                                    .Include("Ratings.User")
                                    .FirstOrDefault(r => r.Id == rating.ResturantId);
                int totalr = 0;
                foreach (var ra in res.Ratings)
                {
                    totalr += ra.RatingN;
                }
                res.Rating = Convert.ToInt32(totalr / (res.Ratings.Count));
                _context.Restaurants.Update(res);
                _context.SaveChanges();
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else {
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }
        public IActionResult Rate() {
            return PartialView("Rating", new Rating());
        }
        
    }
}