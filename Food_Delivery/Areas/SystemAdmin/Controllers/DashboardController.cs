
using Food_Delivery.Areas.Employee.ViewModels;
using Food_Delivery.Helpers;
using Food_Delivery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Areas.SystemAdmin.Controllers
{
    [Area("SystemAdmin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment,SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _signInManager = signInManager;
        }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        [HttpGet]
        public IActionResult Index()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            return View();
        }

        #region ADMIN LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View(new InputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(InputModel input,string returnUrl = null)
        {

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded && await _userManager.IsInRoleAsync(await _userManager.FindByEmailAsync(input.Email), Role.Admin))
                {
                    return RedirectToAction(nameof(ManageEmployees));
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
        [HttpGet]
        public async Task<IActionResult> EmployeeUpsert(string id)
        {
            if (id == null)
            {
                return View(new EmployeeUpsertVM {});
            }


            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeVM = new EmployeeUpsertVM {Id = employee.Id,FirstName = employee.FirstName,LastName = employee.LastName,Email = employee.Email,ImagePath=employee.ImagePath,CVdocumentPath = employee.CVdocumentPath,PhoneNumber = employee.PhoneNumber};
            return View(employeeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeUpsert(string id, EmployeeUpsertVM employeeVM)
        {
            if (employeeVM.Id == null )
            {
                //CREATE
                if (ModelState.IsValid)
                {
                    var employee = new ApplicationUser { UserName = employeeVM.Email, Email = employeeVM.Email, DateCreated = DateTime.Now, FirstName = employeeVM.FirstName, LastName = employeeVM.LastName, PhoneNumber = employeeVM.PhoneNumber };
                    if (employeeVM.CVDocument == null) {
                        ModelState.AddModelError("Document", "You need to attach a CV.");
                        return View(employeeVM);
                    }
                    employee.CVdocumentPath = await ProcessUploadedCVAsync(employeeVM.CVDocument);
                    employee.ImagePath = await ProcessUploadedEImageAsync(employeeVM.Image);
                    var result = await _userManager.CreateAsync(employee, "123456");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(employee, Role.Employee);
                        return RedirectToAction(nameof(Index));
                    }
                    else {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                    //MAYBE ADD A "USER CREATED TOASTER"
                }

            }
            else
            {
                if (ModelState.IsValid)
                {
                    var employee = await _userManager.FindByIdAsync(employeeVM.Id);
                    employee.UserName = employeeVM.Email; employee.UserName = employeeVM.Email; employee.FirstName = employeeVM.FirstName; employee.LastName = employeeVM.LastName; employee.PhoneNumber = employeeVM.PhoneNumber;
                    //var employee = new ApplicationUser {PasswordHash =(await _userManager.FindByIdAsync(employeeVM.Id)).PasswordHash, Id = employeeVM.Id, UserName = employeeVM.Email, Email = employeeVM.Email, DateCreated = employeeVM.DateCreated, FirstName = employeeVM.FirstName, LastName = employeeVM.LastName};

                    if (employeeVM.CVDocument == null)
                    {
                        //Cv not updated
                    }
                    else
                    {
                        //Find the old file
                        string oldFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "DocumentFiles", "CV", employeeVM.CVdocumentPath);
                        System.IO.File.Delete(oldFilePath);
                        employee.CVdocumentPath = await ProcessUploadedCVAsync(employeeVM.CVDocument);
                    }
                    if (employeeVM.Image == null)
                    {
                        //Image not updated
                    }
                    else
                    {

                        //Find the old image
                        if (employeeVM.ImagePath != null)
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Employees", employeeVM.ImagePath);
                            System.IO.File.Delete(oldFilePath);
                        }
                        employee.ImagePath = await ProcessUploadedEImageAsync(employeeVM.Image);
                    }
                   var result =  await _userManager.UpdateAsync(employee);
                    return RedirectToAction(nameof(ManageEmployees));
                }
            }
            return View(employeeVM);
        }

        [HttpGet]
        public IActionResult ManageEmployees() {
            //CHANGE THIS LATTER TO SHOW ONLY EMPLOYES
            var allUsers  = _userManager.Users.ToList();
            return View(allUsers);
        }
        private async Task<string> ProcessUploadedCVAsync(IFormFile sentFile)
        {
            string uniqeFileName = null;
            if (sentFile != null)
            {

                string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "DocumentFiles", "CV");
                uniqeFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("dd_MM_yyyy") + "_" + sentFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqeFileName);
                //System.IO.File.Delete(filePath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await sentFile.CopyToAsync(fileStream);
                }
            }
            return uniqeFileName;
        }
        private async Task<string> ProcessUploadedEImageAsync(IFormFile sentFile)
        {
            string uniqeFileName = null;
            if (sentFile != null)
            {

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Employees");
                uniqeFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("dd_MM_yyyy") + "_" + sentFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqeFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await sentFile.CopyToAsync(fileStream);
                }
            }
            return uniqeFileName;
        }
        private async Task<string> ProcessUploadedRImageAsync(IFormFile sentFile)
        {
            string uniqeFileName = null;
            if (sentFile != null)
            {

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Restaurants");
                uniqeFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("dd_MM_yyyy") + "_" + sentFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqeFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await sentFile.CopyToAsync(fileStream);
                }
            }
            return uniqeFileName;
        }
        private async Task<string> ProcessUploadedFImageAsync(IFormFile sentFile)
        {
            string uniqeFileName = null;
            if (sentFile != null)
            {

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Foods");
                uniqeFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("dd_MM_yyyy") + "_" + sentFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqeFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await sentFile.CopyToAsync(fileStream);
                }
            }
            return uniqeFileName;
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
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        [HttpGet]
        public async Task<IActionResult> GetCVDoc(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (employee.CVdocumentPath == "")
            {
                return NotFound();
            }
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(employee.CVdocumentPath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "DocumentFiles", "CV", employee.CVdocumentPath);
            Response.ContentType = contentType;
            string fname = employee.CVdocumentPath;
            Response.Headers.Append("Content-Disposition", "attachment; filename=" + fname);
            return PhysicalFile(filePath, contentType);
        }
        #endregion

        #region RESTURANTS
        [HttpGet]
        public IActionResult RestaurantUpsert(int id)
        {
            if (id == 0)
            {
                return View(new Restaurant());
            }


            var resturant = _context.Restaurants.FirstOrDefault(res => res.Id == id);
            if (resturant == null)
            {
                return NotFound();
            }
            return View(resturant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>RestaurantUpsert(int id, Restaurant restaurant)
        {
            if (restaurant.Id == 0)
            {
                //CREATE
                if (ModelState.IsValid)
                {
                    if (restaurant.Image == null || restaurant.LargeImage==null)
                    {
                        ModelState.AddModelError("Image", "You need to attach the restaurant images.");
                        return View(restaurant);
                    }
                    restaurant.ImagePath = await ProcessUploadedRImageAsync(restaurant.Image);
                    restaurant.LargeImagePath = await ProcessUploadedRImageAsync(restaurant.LargeImage);
                    await _context.Restaurants.AddAsync(restaurant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageRestaurants));
                }

            }
            else
            {
                if (ModelState.IsValid)
                {

                    if (restaurant.Image == null)
                    {
                        //Image not updated
                    }
                    else
                    {

                        //Find the old image
                        if (restaurant.ImagePath != null)
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Restaurants", restaurant.ImagePath);
                            System.IO.File.Delete(oldFilePath);
                        }
                        restaurant.ImagePath = await ProcessUploadedRImageAsync(restaurant.Image);
                    }
                    if (restaurant.LargeImage == null)
                    {
                        //Image not updated
                    }
                    else
                    {

                        //Find the old image
                        if (restaurant.LargeImagePath != null)
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Restaurants", restaurant.LargeImagePath);
                            System.IO.File.Delete(oldFilePath);
                        }
                        restaurant.LargeImagePath = await ProcessUploadedRImageAsync(restaurant.LargeImage);
                    }

                    var result =  _context.Restaurants.Update(restaurant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageRestaurants));
                }
            }
            return View(restaurant);
        }
        [HttpGet]
        public IActionResult ManageRestaurants()
        {
            var allRestaurants = _context.Restaurants.ToList();
            return View(allRestaurants);
        }
        [HttpGet]
        public IActionResult FoodUpsert(int id) {

            ViewData["Restaurant"] = new SelectList(_context.Restaurants, "Id", "Name");
            if (id == 0)
            {
                return View(new Food());
            }


            var resturant = _context.Restaurants.FirstOrDefault(res => res.Id == id);
            if (resturant == null)
            {
                return NotFound();
            }
            return View(resturant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FoodUpsert(int id, Food food)
        {
            if (food.Id == 0)
            {
                //CREATE
                if (ModelState.IsValid)
                {
                    if (food.Image == null)
                    {
                        ModelState.AddModelError("Image", "You need to attach image of the food.");
                        ViewData["Restaurant"] = new SelectList(_context.Restaurants, "Id", "Name");
                        return View(food);
                    }
                    food.ImagePath = await ProcessUploadedFImageAsync(food.Image);
                    await _context.Foods.AddAsync(food);
                    await _context.SaveChangesAsync();
                    ///HERE
                    return RedirectToAction(nameof(ManageRestaurants));
                }

            }
            else
            {
                if (ModelState.IsValid)
                {

                    if (food.Image == null)
                    {
                        //Image not updated
                    }
                    else
                    {

                        //Find the old image
                        if (food.ImagePath != null)
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Foods", food.ImagePath);
                            System.IO.File.Delete(oldFilePath);
                        }
                        food.ImagePath = await ProcessUploadedRImageAsync(food.Image);
                    }
                    var result = _context.Foods.Update(food);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageRestaurants));
                }
            }
            ViewData["Restaurant"] = new SelectList(_context.Restaurants, "Id", "Name");
            return View(food);
        }
        #endregion
    }
}
