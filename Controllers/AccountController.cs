﻿using LoginLogout.DbContextConn;
using LoginLogout.Entities;
using LoginLogout.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using LoginLogout.Common;

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
            //return View(_context.UserAccounts.ToList());
            var data = _context.UserAccounts.Where(b => (b.Role == 0)).ToList();
            if (data.Count > 0 && data != null)
            {
                return View(data);
            }
            else
            {
                return View(null);
            }
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
                clsGlobal cls = new clsGlobal();
                UserAccount userAccount = new UserAccount();
                userAccount.FirstName = viewModel.FirstName;
                userAccount.LastName = viewModel.LastName;
                userAccount.Email = viewModel.Email;
                userAccount.UserName = viewModel.UserName;
                userAccount.Password = cls.EncryptString(viewModel.Password);
                userAccount.Role = viewModel.Role;

                try
                {
                    _context.UserAccounts.Add(userAccount);
                    _context.SaveChanges();

                    ModelState.Clear();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("Message", "Email Id already exist");
                    string message = ex.Message.ToString();
                    return ViewBag(viewModel);
                }
                ViewBag.Message = $"<b> {userAccount.FirstName} account Created !!</b>";
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
                clsGlobal cls = new clsGlobal();
                var user = _context.UserAccounts.Where(x => (x.UserName == model.UserNameOrEmail || x.Email == model.UserNameOrEmail) && x.Password == cls.EncryptString(model.Password)).FirstOrDefault();

                if (user != null)
                {
                    //Valid user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name",user.FirstName),
                        new Claim(ClaimTypes.Role,user.Role.ToString())
                    };

                    //Verification Admin
                    if (user.Role == 1)
                    {
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Custom");
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

        [Authorize]
        public IActionResult ApproveUser(UserAccount user)
        {
            try
            {
                UserAccount account = new UserAccount();
                account.Id = user.Id;

                var data = _context.UserAccounts.FirstOrDefault(t => t.Id == account.Id);

                if (data != null)
                {
                    data.Role = 1;

                    _context.Update(data);
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult CompleteInformation()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CompleteInformation(UserInformationViewModel userInformation)
        {
             if (ModelState.IsValid)
             {
                 UserInformation user = new UserInformation();
                 user.AddressLine1 = userInformation.AddressLine1;
                 user.AddressLine2 = userInformation.AddressLine2;
                 user.Pincode = userInformation.Pincode;
                 user.PhoneNumber = userInformation.PhoneNumber;

                 try
                 {
                     _context.UserInformation.Add(user);
                     _context.SaveChanges();

                     ModelState.Clear();
                 }
                 catch (DbUpdateException ex)
                 {
                     ModelState.AddModelError("", "Error");
                     string message = ex.Message.ToString();
                     return ViewBag(userInformation);
                 }
                ViewBag.Message = "";
             }
            return View(userInformation);
        }
    }
}
