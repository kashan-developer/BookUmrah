using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UmrahBooking.Controllers
{
    public class YourItineraryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
