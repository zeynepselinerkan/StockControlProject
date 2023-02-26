using Microsoft.AspNetCore.Mvc;

namespace StockControlProject.WebUI.Areas.UserArea.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
