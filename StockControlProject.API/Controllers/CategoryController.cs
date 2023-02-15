using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> _service;

        public CategoryController(IGenericService<Category> service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            //return Ok("Succeeded");
            return Ok(_service.GetAll());
        }
        [HttpGet]
        public IActionResult GetActiveCategories()
        {
          
            return Ok(_service.GetActive());
        }
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
             return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _service.Add(category);
            //return Ok("Successfull");
            return CreatedAtAction("GetCategoryById", new { id=category.Id},category);
        }
    }
}
