using Food_Delivery.Helpers;
using Food_Delivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Controllers
{
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
            return View(new Order());
        }
        [HttpPost]
        public IActionResult CheckOut(Order order)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            //var allEmp = _userManager.GetUsersInRoleAsync();
            var allEmp = _userManager.Users.ToList();
            var rnd = new Random();
            var unluckyEmp = allEmp.OrderBy(x => rnd.Next()).First();
            if (ModelState.IsValid) {

                foreach (Item item in cart)
                {
                    _context.Orders.Add(new Order
                    {
                        Adress = order.Adress,
                        City = order.City,
                        CustomerId = order.CustomerId,
                        DatePlaced = DateTime.Now,
                        DeliveryLoc = order.DeliveryLoc,
                        Delivered = false,
                        Quantity = item.Quantity,
                        FoodId = item.item.Id,
                        OrderNote = order.OrderNote,
                        EmployeeId = unluckyEmp.Id

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
            ViewBag.cart = cart;
            if (ViewBag.cart != null)
            {
                ViewBag.total = cart.Sum(item => item.item.Price * item.Quantity);
            }
            ViewBag.total = 0;
            HttpContext.Session.Remove("cart");
            return View();
        }
    }
}
