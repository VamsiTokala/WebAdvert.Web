using Microsoft.AspNetCore.Mvc;

namespace WebAdvert.Web.Controllers
{
    public class Accounts : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
