using Animals.Infrastructure.Repositories;
using Animals.REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Animals.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnclosuresController : ControllerBase
    {
        private readonly ICrudServiceAsync<EnclosureModel> _service;

        public EnclosuresController(ICrudServiceAsync<EnclosureModel> service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.ReadAllAsync();
            return Ok(list);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var enclosure = await _service.ReadAsync(id);
            if (enclosure == null)
                return NotFound();

            return Ok(enclosure);
        }

        [Authorize(Roles = "Keeper,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EnclosureModel enclosure)
        {
            await _service.CreateAsync(enclosure);
            await _service.SaveAsync();
            return CreatedAtAction(nameof(Get), new { id = enclosure.Id }, enclosure);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EnclosureModel model)
        {
            if (id != model.Id)
                return BadRequest();

            var found = await _service.ReadAsync(id);
            if (found == null)
                return NotFound();

            await _service.UpdateAsync(model);
            await _service.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var enclosure = await _service.ReadAsync(id);
            if (enclosure == null)
                return NotFound();

            await _service.RemoveAsync(enclosure);
            await _service.SaveAsync();
            return NoContent();
        }
    }
}
