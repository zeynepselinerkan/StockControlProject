﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin"), Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
