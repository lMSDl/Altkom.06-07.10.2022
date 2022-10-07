using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ICrudService<User> _service;

        public UsersController(ICrudService<User> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var values = await _service.ReadAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _service.ReadAsync(id);
            if (value == null)
                return NotFound();
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            var id = await _service.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new { id = id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, User user)
        {
            var value = await _service.ReadAsync(id);
            if (value == null)
                return NotFound();

            await _service.UpdateAsync(id, user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var value = await _service.ReadAsync(id);
            if (value == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
