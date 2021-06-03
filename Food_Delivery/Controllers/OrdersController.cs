using Food_Delivery.Helpers;
using Food_Delivery.Models;
using Food_Delivery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Food_Delivery.Controllers
{
    [Authorize(Roles=Role.Customer)]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult CheckOut1()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (ViewBag.cart != null)
            {
                ViewBag.total = cart.Sum(item => item.item.Price * item.Quantity);
                return View();
            }
            ViewBag.total = 0;
            return View(new CheckoutVM());
        }
        [HttpPost]
        public async Task<IActionResult> CheckOutAsync(CheckoutVM orders)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            var allEmp = await _userManager.GetUsersInRoleAsync(Role.Employee);
            var rnd = new Random();
            var unluckyEmp = allEmp.OrderBy(x => rnd.Next()).First();
            if (ModelState.IsValid) {

                var newBatch = new OrderBatch
                {
                    Adress = orders.Adress,
                    City = orders.City,
                    CustomerId = orders.CustomerId,
                    DatePlaced = DateTime.Now,
                    DeliveryLoc = orders.DeliveryLoc,
                    Delivered = false,
                    OrderNote = orders.OrderNote,
                    EmployeeId = unluckyEmp.Id

                };
                _context.OrderBatches.Add(newBatch);
                await _context.SaveChangesAsync();
                foreach (Item item in cart)
                {
                    _context.Orders.Add(new Order
                    {
                        Quantity = item.Quantity,
                        FoodId = item.item.Id,
                        BatchId = newBatch.Id

                    });
                }
                _context.SaveChanges();
                return RedirectToAction(actionName: nameof(CheckOut2));
            }
            return RedirectToAction(actionName: nameof(CheckOut1));
        }
        [HttpGet]
        public IActionResult CheckOut2()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            HttpContext.Session.Remove("cart");
            ViewBag.cart = cart;
            if (ViewBag.cart != null)
            {
                ViewBag.total = cart.Sum(item => item.item.Price * item.Quantity);
                return View();
            }
            ViewBag.total = 0;
            return View();

        }
    }
}
