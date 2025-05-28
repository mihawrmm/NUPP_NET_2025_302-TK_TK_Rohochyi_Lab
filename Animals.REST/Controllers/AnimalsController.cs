using Animals.REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Animals.Infrastructure.Repositories;

namespace Animals.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly ICrudServiceAsync<AnimalModel> _service;

        public AnimalsController(ICrudServiceAsync<AnimalModel> service)
        {
            _service = service;
        }

        // GET: api/animals
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var animals = await _service.ReadAllAsync();

            var result = animals.Select(a => new REST.Models.AnimalModel
            {
                Id = a.Id,
                Name = a.Name,
                Age = a.Age,
                EnclosureId = a.EnclosureId
            });

            return Ok(result);
        }

        // GET: api/animals/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var animal = await _service.ReadAsync(id);
            if (animal == null)
                return NotFound();

            var result = new REST.Models.AnimalModel
            {
                Id = animal.Id,
                Name = animal.Name,
                Age = animal.Age,
                EnclosureId = animal.EnclosureId
            };

            return Ok(result);
        }

        // POST: api/animals
        [Authorize(Roles = "Keeper,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] REST.Models.AnimalModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new AnimalModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Age = model.Age,
                EnclosureId = model.EnclosureId
            };

            await _service.CreateAsync(entity);
            await _service.SaveAsync();

            model.Id = entity.Id;

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: api/animals/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] REST.Models.AnimalModel model)
        {
            if (id != model.Id)
                return BadRequest();

            var existing = await _service.ReadAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = model.Name;
            existing.Age = model.Age;
            existing.EnclosureId = model.EnclosureId;

            await _service.UpdateAsync(existing);
            await _service.SaveAsync();

            return NoContent();
        }

        // DELETE: api/animals/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var animal = await _service.ReadAsync(id);
            if (animal == null)
                return NotFound();

            await _service.RemoveAsync(animal);
            await _service.SaveAsync();

            return NoContent();
        }
    }
}
