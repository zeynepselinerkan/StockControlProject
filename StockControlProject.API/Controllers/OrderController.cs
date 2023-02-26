using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockControlProject.Domain.Entities;
using StockControlProject.Domain.Enums;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> _service;

        public IGenericService<Order> _orderService;
        public IGenericService<Product> _productService;
        public IGenericService<OrderDetails> _orderDetailsService;
        public IGenericService<User> _userService;

        public OrderController(IGenericService<Order> orderService, IGenericService<Product> productService, IGenericService<OrderDetails> orderDetailsService, IGenericService<User> userService)
        {
            _orderService = orderService;
            _productService = productService;
            _orderDetailsService = orderDetailsService;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(_orderService.GetAll(t0 => t0.OrderDetails, t1 => t1.User));
        }
        [HttpGet]
        public IActionResult GetActiveOrders()
        {

            return Ok(_orderService.GetActive(t0 => t0.OrderDetails, t1 => t1.User));
        }
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            return Ok(_orderService.GetById(id,t0 => t0.OrderDetails, t1 => t1.User));
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderDetailById(int id)
        {
            return Ok(_orderDetailsService.GetAll(x=> x.OrderId==id, t1 => t1.Product));
        }

        [HttpGet]
        public IActionResult GetPendingOrders(Order order)
        {
            return Ok(_orderService.GetDefault(x=>x.Status==Status.Pending));
        }
        [HttpGet]
        public IActionResult GetRejectedOrders(Order order)
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Cancelled));
        }
        [HttpPost]
        public IActionResult AddOrder(int userId, [FromQuery] int[] productIds, [FromQuery] short[] quantities)
        {
            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.Status = Status.Pending; // Eklenen sipariş bekliyor durumunda eklenecek.
            newOrder.IsActive = true; // Sipariş onaylanır ya da reddedilirse false'a çekilecek.

            _orderService.Add(newOrder); // DB'ye eklendiğinde yeni bir order satırı ekleniyor ve Id oluşuyor.
            //int[] productIds = new int {1,3,... };

            for (int i = 0; i < productIds.Length; i++)
            {
                OrderDetails newDetail = new OrderDetails();
                newDetail.OrderId = newOrder.Id;
                newDetail.ProductId = productIds[i];
                newDetail.Quantity = quantities[i];
                newDetail.UnitPrice = _productService.GetById(productIds[i]).UnitPrice;
                newDetail.IsActive = true;

                _orderDetailsService.Add(newDetail);
            }
            return CreatedAtAction("GetOrderById", new { id = newOrder.Id }, newOrder);
        }
        [HttpGet("{id}")]
        public IActionResult ConfirmOrder(int id) // Stoğu düşürmem anlamına geliyor. Stok miktarından fazla giriş yapılamaması kısmı UI katmanında yapılacak. Burada gerek yok.
        {
            Order confirmedOrder = _orderService.GetById(id);
            if (confirmedOrder==null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetails> details = _orderDetailsService.GetDefault(x => x.OrderId ==confirmedOrder.Id).ToList();

                foreach (OrderDetails item in details) // 1 ordertaki tüm orderdetail'lar okunuyor.
                {
                    Product productInOrder = _productService.GetById(item.ProductId);
                    productInOrder.Stock -= item.Quantity;
                    _productService.Update(productInOrder);

                    item.IsActive = false;
                    _orderDetailsService.Update(item);
                }
            }
            confirmedOrder.Status = Status.Confirmed;
            confirmedOrder.IsActive = false;
            _orderService.Update(confirmedOrder);

            return Ok(confirmedOrder);
        }
        [HttpGet("{id}")]
        public IActionResult RejectOrder(int id) 
        {
            Order rejectedOrder = _orderService.GetById(id);
            if (rejectedOrder == null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetails> details = _orderDetailsService.GetDefault(x => x.OrderId == rejectedOrder.Id).ToList();

                foreach (OrderDetails item in details) 
                {
                    //Product productInOrder = _productService.GetById(item.ProductId);
                    //productInOrder.Stock -= item.Quantity;
                    //_productService.Update(productInOrder);

                    item.IsActive = false;
                    _orderDetailsService.Update(item);
                }
            }
            rejectedOrder.Status = Status.Cancelled;
            rejectedOrder.IsActive = false;
            _orderService.Update(rejectedOrder);

            return Ok(rejectedOrder);
        }

    }
}
