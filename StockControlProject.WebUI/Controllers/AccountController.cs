using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;
using StockControlProject.Domain.Enums;
using StockControlProject.WebUI.Models;
using System.Security.Claims;

namespace StockControlProject.WebUI.Controllers
{
    public class AccountController : Controller
    {
        string uri = "https://localhost:7182";
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            User logged = new User();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/User/Login?email={dto.Email}&password={dto.Password}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    logged = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            if (logged!=null)
            {
                var claims = new List<Claim>()
                {
                    new Claim("Id",logged.Id.ToString()),
                    new Claim("CompanyId",logged.CompanyId.ToString()),
                    new Claim(ClaimTypes.Name,logged.FirstName),
                    new Claim(ClaimTypes.Surname,logged.LastName),
                    new Claim(ClaimTypes.Email,logged.Email),
                    new Claim(ClaimTypes.Role,logged.Role.ToString()),
                    new Claim("Image",value : logged.PhotoURL)
                };

                var userIdentity = new ClaimsIdentity(claims,"login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }
            else
            {
                return View(dto); // Kullanıcı giriş yapamazsa ilgili bilgilerle geri dönsün.
            }

            switch (logged.Role)
            {
                case UserRole.Admin:
                    return RedirectToAction("Index","Home",new {Area="Admin"});
                case UserRole.Supplier:
                    return RedirectToAction("Index", "Home", new { Area = "SupplierArea" });
                case UserRole.User:
                    return RedirectToAction("Index", "Home", new { Area = "UserArea" });
                default:
                    return View(dto);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account", new { Area = "" });
        }
    }
}
