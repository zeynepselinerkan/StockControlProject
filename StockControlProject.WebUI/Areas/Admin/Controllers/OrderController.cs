using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        string uri = "https://localhost:7182";
        public async Task<IActionResult> Index()
        {
            // Tüm siparişler gelsin.

            List<Order> orders = new List<Order>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Order/GetAllOrders"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Json isteği stringe çeviriyor.
                    orders = JsonConvert.DeserializeObject<List<Order>>(apiResponse); // Siparişler listesine dönüşüyor.(NET type'ına)

                    //Elimde Json olarak okunmuş siparişler listem var.
                }
            }
            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Order/ConfirmOrder/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> RejectOrder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Order/RejectOrder/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        // TO DO : Sipariş detayını gör(OrderDetails) action metodu yazılacak. Bu viewda ise siprişin içindeki orderdetailstan gelen ürünler ve detay bilgileri yazacak. Aynı zamanda OD içinde olan sayı * birim fiyat üzerinden o ürünün adedine göre toplam tutar kolonu da eklenecek. Alta da tüm ürünlerin adetlerine göre hesaplanmış genel toplam yazdırılabilir.
        public async Task<IActionResult> OrderDetails(int id)
        {
            List<OrderDetails> orderDetails = new List<OrderDetails>();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"{uri}/api/Order/GetOrderDetailsById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    orderDetails = JsonConvert.DeserializeObject<List<OrderDetails>>(apiResponse)!;
                }
            }

            return View(orderDetails);
        }




        //[HttpGet]
        //public async Task<IActionResult> OrderDetails(int id)
        //{

        //    List<OrderDetails> orderDetails = new List<OrderDetails>();

        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = await httpClient.GetAsync($"{uri}/api/Order/GetOrderById/{id}"))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync(); 
        //            orderDetails = JsonConvert.DeserializeObject<List<OrderDetails>>(apiResponse);


        //        }
        //    }
        //    return View(orderDetails);



    }
}
