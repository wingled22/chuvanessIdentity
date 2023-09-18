using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using sampleApp.Entities;
using sampleApp.Identity;

namespace sampleApp.Controllers
{
    public class AuthedController : Controller
    {
        private  UserManager<ApplicationUser> _usrMngr;
        private  SignInManager<ApplicationUser> _signInMngr;
        private readonly SampleappContext _context;

        public AuthedController(UserManager<ApplicationUser> usrMngr, SignInManager<ApplicationUser> signInMngr, SampleappContext context)
        {
            _usrMngr = usrMngr;
            _signInMngr = signInMngr;
            _context = context;
        }
        public IActionResult Index(){
            if(!User.Identity.IsAuthenticated){
                return RedirectToAction("Login","Account");
            }
            return View();
        }

    }
}