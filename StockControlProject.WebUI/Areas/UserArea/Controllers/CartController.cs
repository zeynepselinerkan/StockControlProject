using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;
using System;
using System.Text;

namespace StockControlProject.WebUI.Areas.UserArea.Controllers
{
    [Area("UserArea"),Authorize(Roles ="User")]
    public class CartController : Controller
    {
        string uri = "https://localhost:7182";
        static CartController()
        {
            addedToCart = new List<Product>();
        }
        public IActionResult Index()
        {
            return View(addedToCart);
        }

        static List<Product> addedToCart;
        static Product product;

        [HttpGet]
        public async Task<IActionResult> AddToCart(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Product/GetProductById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                     product =JsonConvert.DeserializeObject<Product>(apiResponse);
                    
                }
                
            }
            addedToCart.Add(product);
            return RedirectToAction(nameof(Index));
        }

        // TO DO : Siparişi tamamla actionı eklenecek. Bu action APIdeki SiparisEkle actionını tetikleyecek.

        public async Task<IActionResult> CompleteOrder()
        {
            int[] productIds = new int[0];
            short[] quantitites = new short[0];
            for (int i = 0; i < addedToCart.Count; i++)
            {
                Array.Resize(ref productIds, productIds.Length + 1);
                Array.Resize(ref quantitites, quantitites.Length + 1);
                productIds[i] = addedToCart[i].Id;
                quantitites[i] = 1;
            }
            string toGoIds = "";
            foreach (var item in productIds)
            {
                toGoIds += "&productIds=" + item;
            }

            string toGoQuantities = "";
            foreach (var item in productIds)
            {
                toGoQuantities += "&quantities=" + item;
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                
                using (var response = await httpClient.PostAsJsonAsync($"{uri}/api/Order/AddOrder?userId={HttpContext.User.FindFirst("Id").Value}{toGoIds}{toGoQuantities}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));

            // MVC DEKİ COMPLETEORDER DA SEPETTE YENİ SÜTUN MİKTAR ARTTIR AZALT
            // KENDİ SİPARİŞLERİMİ GÖREBİLECEĞİM ACTİON VE VIEWLARI
        }
    }
}
