using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> _service;

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Login(string email, string password)
        {
            if (_service.Any(user=> user.Email==email && user.Password==password))
            {
                User loggedUser = _service.GetByDefault(user => user.Email == email && user.Password == password);
                return Ok(loggedUser);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {

            return Ok(_service.GetAll());
        }
        [HttpGet]
        public IActionResult GetActiveUsers()
        {

            return Ok(_service.GetActive());
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _service.Add(user);
            return CreatedAtAction("GetUserById", new { id = user.Id }, user);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            if (!IsThereUser(id))
            {
                return NotFound();
            }
            try
            {
                _service.Update(user);
                return Ok(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
        private bool IsThereUser(int id) // Overload ile email, password verirsek bu methodla Login'i oluşturabiliriz.
        {
            return _service.Any(user => user.Id == id);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound();
            try
            {
                _service.Remove(user);
                return Ok("User was deleted.");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult ActivateUser(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                //return Ok("User was activated.");
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
