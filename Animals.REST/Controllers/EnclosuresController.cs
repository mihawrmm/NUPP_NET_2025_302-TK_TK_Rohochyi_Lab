using Animals.Infrastructure.Repositories;
using Animals.REST.Models;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.ReadAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var enclosure = await _service.ReadAsync(id);
            if (enclosure == null)
                return NotFound();

            return Ok(enclosure);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EnclosureModel enclosure)
        {
            await _service.CreateAsync(enclosure);
            await _service.SaveAsync();
            return CreatedAtAction(nameof(Get), new { id = enclosure.Id }, enclosure);
        }

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
