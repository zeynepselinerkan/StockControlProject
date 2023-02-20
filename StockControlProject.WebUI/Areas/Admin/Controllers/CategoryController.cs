using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        string uri = "https://localhost:7182";
        public async Task<IActionResult> Index()
        {
            // Tüm kategoriler gelsin.

            List<Category> categories = new List<Category>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Category/GetAllCategories"))
                {
                    string apiResponse=await response.Content.ReadAsStringAsync(); // Json isteği stringe çeviriyor.
                    categories = JsonConvert.DeserializeObject<List<Category>>(apiResponse); // Kategoriler listesine dönüşüyor.(NET type'ına)

                    //Elimde Json olarak okunmuş kategoriler listem var.
                }
            }
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> ActivateCategory(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Category/ActivateCategory/{id}"))
                {
                   
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{uri}/api/Category/DeleteCategory/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
    }
}
