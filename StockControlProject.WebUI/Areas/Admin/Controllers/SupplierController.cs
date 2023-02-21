using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;
using System.Text;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        string uri = "https://localhost:7182";
        public async Task<IActionResult> Index()
        {
            // Tüm tedarikçiler gelsin.

            List<Supplier> suppliers = new List<Supplier>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Supplier/GetAllSuppliers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Json isteği stringe çeviriyor.
                    suppliers = JsonConvert.DeserializeObject<List<Supplier>>(apiResponse); // Tedarikçiler listesine dönüşüyor.(NET type'ına)

                    //Elimde Json olarak okunmuş tedarikçiler listem var.
                }
            }
            return View(suppliers);
        }
        [HttpGet]
        public async Task<IActionResult> ActivateSupplier(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Supplier/ActivateSupplier/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{uri}/api/Supplier/DeleteSupplier/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddSupplier() { return View(); }
        [HttpPost]
        public async Task<IActionResult> AddSupplier(Supplier supplier)
        {
            supplier.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json"); // Türkçe karakterini destekleyen bir json oluşturdum.

                using (var response = await httpClient.PostAsync($"{uri}/api/Supplier/AddSupplier", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        static Supplier updatedSupplier;
        [HttpGet]
        public async Task<IActionResult> UpdateSupplier(int id) // id ile ilgili tedarikçiyi bul ve viewda göster.
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Supplier/GetSupplierById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    updatedSupplier = JsonConvert.DeserializeObject<Supplier>(apiResponse);
                }
            }
            return View(updatedSupplier);  // İlgili nesne ile ilgili güncelleme View'ını göstercek.
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSupplier(Supplier supplierUp)
        {

            using (var httpClient = new HttpClient())
            {
                supplierUp.AddedDate = updatedSupplier.AddedDate;
                supplierUp.IsActive = true; // İstemiyorsak =updatedSupplier.IsActive; yazarım. Hiç yazmazsam defaultu getirir. Tarihte falan sorun yaşarım.

                StringContent content = new StringContent(JsonConvert.SerializeObject(supplierUp), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{uri}/api/Supplier/UpdateSupplier/{supplierUp.Id}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Cevabı almak için
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}