using LoginLogout.DbContextConn;
using LoginLogout.Entities;
using LoginLogout.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoginLogout.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                UserAccount userAccount = new UserAccount();
                userAccount.FirstName = viewModel.FirstName;
                userAccount.LastName = viewModel.LastName;
                userAccount.Email = viewModel.Email;
                userAccount.UserName = viewModel.UserName;
                userAccount.Password = viewModel.Password;

                try
                {
                    _context.UserAccounts.Add(userAccount);
                    _context.SaveChanges();

                    ModelState.Clear();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Email Id already exist");
                    string message = ex.Message.ToString();
                    return ViewBag(viewModel);
                }
                ViewBag.Message = $"{userAccount.FirstName} account Created !!";
            }
            return View(viewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccounts.Where(x => (x.UserName == model.UserNameOrEmail || x.Email == model.UserNameOrEmail) && x.Password == model.Password).FirstOrDefault();

                if (user != null)
                {
                    //Valid user
                    var claims = new List<Claim> 
                    { 
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name",user.FirstName),
                        new Claim(ClaimTypes.Role,user.Role)
                    };


                    if (user.Role.Equals("admin"))
                    {
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Dashboard");
                    }


                }
                else
                {
                    //Incorrect Credentials or Invalid User
                    ModelState.AddModelError("", "Username/Email or Password is not correct");
                }
            }

            return View(model);
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
