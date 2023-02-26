using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;
using System.Text;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        string uri = "https://localhost:7182";
        public async Task<IActionResult> Index()
        {
            // Tüm ürünler gelsin.

            List<Product> products = new List<Product>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Product/GetAllProducts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Json isteği stringe çeviriyor.
                    products = JsonConvert.DeserializeObject<List<Product>>(apiResponse); // Ürünler listesine dönüşüyor.(NET type'ına)

                    //Elimde Json olarak okunmuş ürünler listem var.
                }
            }
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> ActivateProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Product/ActivateProduct/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{uri}/api/Product/DeleteProduct/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        static List<Category> activeCategories;
        static List<Supplier> activeSuppliers;
        [HttpGet]
        public async Task<IActionResult> AddProduct() 
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Category/GetActiveCategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); 
                    activeCategories = JsonConvert.DeserializeObject<List<Category>>(apiResponse); 

                }
            }
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Supplier/GetActiveSuppliers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    activeSuppliers = JsonConvert.DeserializeObject<List<Supplier>>(apiResponse); 
                }
            }

            ViewBag.ActiveCategories = new SelectList(activeCategories, "Id", "CategoryName");
            ViewBag.ActiveSuppliers = new SelectList(activeSuppliers, "Id", "SupplierName");

            return View(); // Ekleme viewını gösterirken Viewbagler ile select listlere aktif kategorileri ve aktif tedarikçileri göndermek istiyorum.
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            product.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{uri}/api/Product/AddProduct", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        static Product updatedProduct;
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id) // id ile ilgili ürünü bul ve viewda göster.
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Product/GetProductById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    updatedProduct = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
                using (var response = await httpClient.GetAsync($"{uri}/api/Category/GetActiveCategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    activeCategories = JsonConvert.DeserializeObject<List<Category>>(apiResponse);

                }
                using (var response = await httpClient.GetAsync($"{uri}/api/Supplier/GetActiveSuppliers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    activeSuppliers = JsonConvert.DeserializeObject<List<Supplier>>(apiResponse);
                }

            }

            ViewBag.ActiveCategories = new SelectList(activeCategories, "Id", "CategoryName");
            ViewBag.ActiveSuppliers = new SelectList(activeSuppliers, "Id", "SupplierName");
            return View(updatedProduct);  // İlgili nesne ile ilgili güncelleme View'ını göstercek.
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product productUp)
        {

            using (var httpClient = new HttpClient())
            {
                productUp.AddedDate = updatedProduct.AddedDate;
                productUp.IsActive = updatedProduct.IsActive; // Hiç yazmazsam defaultu getirir. Tarihte falan sorun yaşarım.

                StringContent content = new StringContent(JsonConvert.SerializeObject(productUp), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{uri}/api/Product/UpdateProduct/{productUp.Id}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Cevabı almak için
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 
       