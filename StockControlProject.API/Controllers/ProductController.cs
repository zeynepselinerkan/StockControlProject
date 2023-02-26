using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> _service;

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(_service.GetAll(t0 => t0.Category,t1=>t1.Supplier));
        }
        [HttpGet("{id}")]
        public IActionResult GetSuppliersProducts(int id)
        {
            return Ok(_service.GetAll(x=>x.SupplierId==id,t0 => t0.Category, t1 => t1.Supplier));
        }
        [HttpGet]
        public IActionResult GetActiveProducts()
        {

            return Ok(_service.GetActive(t0 => t0.Category, t1 => t1.Supplier));
        }
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _service.Add(product);
            return CreatedAtAction("GetProductById", new { id = product.Id }, product);;
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            if (!IsThereProduct(id))
            {
                return NotFound();
            }
            try
            {
                _service.Update(product);
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
        private bool IsThereProduct(int id)
        {
            return _service.Any(pr => pr.Id == id);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _service.GetById(id);
            if (product == null)
                return NotFound();
            try
            {
                _service.Remove(product);
                return Ok("Product was deleted.");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult ActivateProduct(int id)
        {
            var product = _service.GetById(id);
            if (product == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                //return Ok("Product was activated.");
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
