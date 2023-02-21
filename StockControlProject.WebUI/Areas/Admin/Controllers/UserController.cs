using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;
using System.Text;
using System;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        string uri = "https://localhost:7182";
        public async Task<IActionResult> Index()
        {
            // Tüm kullanıcılar gelsin.

            List<User> users = new List<User>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/User/GetAllUsers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Json isteği stringe çeviriyor.
                    users = JsonConvert.DeserializeObject<List<User>>(apiResponse); // Kullanıcılar listesine dönüşüyor.(NET type'ına)

                    //Elimde Json olarak okunmuş kullanıcılar listem var.
                }
            }
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> ActivateUser(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/User/ActivateUser/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{uri}/api/User/DeleteUser/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddUser() { return View(); }
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            user.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"); // Türkçe karakterini destekleyen bir json oluşturdum.

                using (var response = await httpClient.PostAsync($"{uri}/api/User/AddUser", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        static User updatedUser;
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id) // id ile ilgili userı bul ve viewda göster.
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/User/GetUserById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    updatedUser = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(updatedUser);  // İlgili nesne ile ilgili güncelleme View'ını göstercek.
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(User userUp)
        {

            using (var httpClient = new HttpClient())
            {
                userUp.AddedDate = updatedUser.AddedDate;
                userUp.IsActive = true; // İstemiyorsak =updatedUser.IsActive; yazarım. Hiç yazmazsam defaultu getirir. Tarihte falan sorun yaşarım.

                StringContent content = new StringContent(JsonConvert.SerializeObject(userUp), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{uri}/api/User/UpdateUser/{userUp.Id}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Cevabı almak için
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
