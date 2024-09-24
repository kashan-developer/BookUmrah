using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UmrahBooking.Controllers
{
    public class VisaController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
