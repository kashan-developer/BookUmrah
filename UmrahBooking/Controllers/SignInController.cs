using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UmrahBooking.Models;

namespace UmrahBooking.Controllers
{
    public class SignInController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataBaseContext _context;

        public SignInController(UserManager<User> userManager, SignInManager<User> signInManager, DataBaseContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //model.Username = "FarazAhmad";
            var status = new Status();
            var user = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();
            //var user = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid UserName or Password";
                return View("Index", status.Message);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid UserName or Password";
                return View("Index", status.Message);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View("Index");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "SignIn");
        }

    }
}
