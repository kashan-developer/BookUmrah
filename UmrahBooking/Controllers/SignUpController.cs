using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UmrahBooking.Models;

namespace UmrahBooking.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public SignUpController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        //[AllowAnonymous]

        //[Route("RegisterUser")]

        public async Task<IActionResult> Register(User model)
        {
            try
            {
                if (ModelState.IsValid)

                {

                    var user = new User { Email = model.Email, Password = model.Password, UserName = model.FirstName+model.LastName,Title = model.Title,FirstName=model.FirstName,LastName=model.LastName,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now };

                    var result = await _userManager.CreateAsync(user, model.Password);



                    if (result.Succeeded)

                    {

                        // Handle successful registration

                        return RedirectToAction("Index", "SignIn");

                    }

                    else

                    {

                        return new BadRequestObjectResult("There was an error processing your request, please try again.");

                    }

                }
                return Json(false);

            }

            catch (Exception ex)

            {

                return new BadRequestObjectResult(ex.Message);

                throw;

            }

        }
    }
}
