using Microsoft.AspNetCore.Mvc;

namespace StockControlProject.WebUI.Areas.UserArea.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
