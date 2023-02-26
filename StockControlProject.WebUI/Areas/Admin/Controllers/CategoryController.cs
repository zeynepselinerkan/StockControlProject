using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;
using System.Text;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
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
        [HttpGet]
        public IActionResult AddCategory() { return View(); }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            category.IsActive = true;

            using(var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json"); // Türkçe karakterini destekleyen bir json oluşturdum.

                using (var response = await httpClient.PostAsync($"{uri}/api/Category/AddCategory", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();  
                }
            }
            return RedirectToAction(nameof(Index));
        }
        static Category updatedCategory;
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id) // id ile ilgili kategoriyi bul ve viewda göster.
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Category/GetCategoryById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); 
                    updatedCategory = JsonConvert.DeserializeObject<Category>(apiResponse); 
                }
            }
            return View(updatedCategory);  // İlgili nesne ile ilgili güncelleme View'ını göstercek.
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category categoryUp)
        {

            using (var httpClient = new HttpClient())
            {
                categoryUp.AddedDate = updatedCategory.AddedDate;
                categoryUp.IsActive=true; // İstemiyorsak =updatedCategory.IsActive; yazarım. Hiç yazmazsam defaultu getirir. Tarihte falan sorun yaşarım.

                StringContent content = new StringContent(JsonConvert.SerializeObject(categoryUp), Encoding.UTF8, "application/json"); 

                using (var response = await httpClient.PutAsync($"{uri}/api/Category/UpdateCategory/{categoryUp.Id}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Cevabı almak için
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
