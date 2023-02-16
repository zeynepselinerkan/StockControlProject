using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id,Category category)
        {
            if (id!=category.Id)
            {
                return BadRequest();
            }
            try
            {
                _service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsThereCategory(id))
                {
                    return NotFound();
             
                }
                return NoContent();
            }
        }
        private bool IsThereCategory(int id)
        {
            return _service.Any(cat => cat.Id == id); // Parametrede gelen id'ye göre kategori var ise true yoksa false dönecektir.
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _service.GetById(id);
            if (category==null)
                return NotFound();
            try
            {
                _service.Remove(category);
                return Ok("Category was deleted.");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult ActivateCategory(int id)
        {
            var category = _service.GetById(id);
            if (category == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                //return Ok("Category was activated.");
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
