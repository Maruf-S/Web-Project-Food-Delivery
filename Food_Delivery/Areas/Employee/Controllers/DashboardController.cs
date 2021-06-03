using Food_Delivery.Helpers;
using Food_Delivery.Models;
using Food_Delivery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles=Role.Employee)]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.DeliveredOrders = _context.OrderBatches
                    .Where(e => e.Delivered == true).Count();
            return View();
        }
        #region Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            //var xre = _userManager.GetUsersInRoleAsync("tutor");
            return View(new InputModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(InputModel input, string returnUrl = null)
        {

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded && await _userManager.IsInRoleAsync(await _userManager.FindByEmailAsync(input.Email), Role.Employee))
                {
                    return RedirectToAction(nameof(Index));
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(input);
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }
        #endregion

        #region Orders
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var curUser = await GetCurrentUserAsync();
            var allOrders =  _context.OrderBatches

                                .Where(e => e.EmployeeId == curUser.Id)
                                .Where(e => e.Delivered == false)
                                .Include(e => e.Employee)
                                .Include(e => e.Customer)
                                .Include(e => e.OrdersList)
                                .Include("OrdersList.Food")
                                .ToList();

            return View(allOrders);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmDelivery(int id)
        {
            var curUser = await GetCurrentUserAsync();
            var allOrders = _context.OrderBatches
                                .Where(e => e.Id == id)
                                .FirstOrDefault();
            allOrders.Delivered = true;
            _context.OrderBatches.Update(allOrders);
            await _context.SaveChangesAsync();

            return RedirectToAction(actionName:nameof(Orders));
        }
        #endregion
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
