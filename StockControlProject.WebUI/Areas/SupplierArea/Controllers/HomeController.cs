using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;

namespace StockControlProject.WebUI.Areas.SupplierArea.Controllers
{
    [Area("SupplierArea"),Authorize(Roles ="Supplier")]
    public class HomeController : Controller
    {
        string uri = "https://localhost:7182";
        public async Task<IActionResult> Index()
        {
            List<Product> products = new List<Product>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Product/GetSuppliersProducts/{HttpContext.User.FindFirst("CompanyId")?.Value.ToString()}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }
            return View(products);
        }
        // TO DO : Ürün ekleme, güncelleme ve silme işlemlerini supplierda yapabilir. Dolayısıyla ilgili action ve viewlar eklenecek (adminden).
    }
}
