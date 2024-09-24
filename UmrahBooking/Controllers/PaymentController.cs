using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UmrahBooking.Controllers
{
    public class PaymentController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
