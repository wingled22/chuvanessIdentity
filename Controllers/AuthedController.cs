using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using sampleApp.Identity;

namespace sampleApp.Controllers
{
    [Authorize]
    public class AuthedController : Controller
    {
        
     private UserManager<ApplicationUser> _usrMngr;
    private SignInManager<ApplicationUser> _signInMngr;

    public AuthedController(UserManager<ApplicationUser> usrMngr,SignInManager<ApplicationUser> signInMngr)
    {
        _usrMngr = usrMngr;
        _signInMngr = _signInMngr;
        
    }
        public IActionResult Index(){
            return View();
        }

        
    }
}