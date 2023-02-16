using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> _service;

        public SupplierController(IGenericService<Supplier> service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            //return Ok("Succeeded");
            return Ok(_service.GetAll());
        }
        [HttpGet]
        public IActionResult GetActiveSuppliers()
        {

            return Ok(_service.GetActive());
        }
        [HttpGet("{id}")]
        public IActionResult GetSupplierById(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult AddSupplier(Supplier supplier)
        {
            _service.Add(supplier);
            //return Ok("Successfull");
            return CreatedAtAction("GetSupplierById", new { id = supplier.Id }, supplier);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest();
            }
            if (!IsThereSupplier(id))
            {
                return NotFound();
            }
            try
            {
                _service.Update(supplier);
                return Ok(supplier);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
        private bool IsThereSupplier(int id)
        {
            return _service.Any(sup => sup.Id == id); // Parametrede gelen id'ye göre kategori var ise true yoksa false dönecektir.
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _service.GetById(id);
            if (supplier == null)
                return NotFound();
            try
            {
                _service.Remove(supplier);
                return Ok("Supplier was deleted.");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult ActivateSupplier(int id)
        {
            var supplier = _service.GetById(id);
            if (supplier == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                //return Ok("Supplier was activated.");
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
