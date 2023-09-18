using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sampleApp.Entities;
using sampleApp.Identity;
using sampleApp.ViewModel;

namespace sampleApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private  UserManager<ApplicationUser> _usrMngr;
        private  SignInManager<ApplicationUser> _signInMngr;
        private readonly SampleappContext _context;

        private readonly ILogger<AccountController> _logger;
        //LMSCContext context,

        public AccountController(IMapper mapper, SampleappContext context, UserManager<ApplicationUser> usrMngr, SignInManager<ApplicationUser> signInMngr,  ILogger<AccountController> logger)
        {
            _mapper = mapper;
            _context = context;
            _usrMngr = usrMngr;
            _signInMngr = signInMngr;
            _logger = logger;

        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;

                if (currentUser.IsInRole("Admin"))
                {
                    _logger.LogInformation("Logged in usertype : ADMIN");

                    // return RedirectToAction("Index", "Page", new { area = "Admin" });
                }

                return RedirectToAction("Index", "Authed");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        Email = registration.Email.ToString(),
                        UserName = registration.UserName.ToString(),
                    };
                    // Console.WriteLine($"Email: {user.Email}");
                    // Console.WriteLine($"Username: {user.UserName}");
                    var result = await _usrMngr.CreateAsync(user, registration.Password);
                    if (result.Succeeded)
                    {
                        _usrMngr.AddToRoleAsync(user, "Admin").Wait();
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            if (item.Code == "DuplicateUserName")
                                ModelState.AddModelError("DuplicateUserName", "Username already taken");
                            if (item.Code == "DuplicateEmail")
                                ModelState.AddModelError("DuplicateEmail", "Email already Registered");
                        }
                        View("Index",registration);
                    }
                }
                return View("Index",registration);
            }
            catch (Exception )
            {
                return View("Index",registration);
            }
        }


        [HttpGet]
        public ActionResult Login(string ReturnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
                return View();

            return RedirectToAction("Index", "Authed");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel login, string Returnurl = null)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Aspnetusers.Where(q => q.Email == login.Email).FirstOrDefault();
                var appUser = _mapper.Map<ApplicationUser>(user);

                if (user != null)
                {
                    var result = await _signInMngr.PasswordSignInAsync(appUser, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Authed");
                    }
                    else
                    {
                        ModelState.AddModelError("passwordAndEmailNotMatch ", "Email and password didn't match");
                        return View("Login",login);
                    }
                }
                else
                {
                    ModelState.AddModelError("noEmailFind ", "No email found.");
                    return View(login);
                }
            }
            return View();
        }

        [Route("Account/Logout")]
        [Route("Admin/Account/Logout")]
        [Route("Student/Account/Logout")]
        //[Route("Teacher/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInMngr.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }



    
    }
}