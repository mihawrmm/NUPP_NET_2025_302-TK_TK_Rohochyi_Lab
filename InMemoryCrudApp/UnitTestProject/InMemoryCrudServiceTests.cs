using System;
using System.Threading.Tasks;
using InMemoryCrudApp.Models;
using InMemoryCrudApp.Services; 
using Xunit; 

namespace UnitTestProject
{
    public class InMemoryCrudServiceTests
    {
        private readonly InMemoryCrudService<Animal> _service;

        public InMemoryCrudServiceTests()
        {
            _service = new InMemoryCrudService<Animal>("test_animals.json");
        }

        // Тест для CreateAsync
        [Fact]
        public async Task CreateAsync_ShouldAddElement()
        {
            var animal = Animal.CreateNew();
            var result = await _service.CreateAsync(animal);

            Assert.True(result); 
        }

        // Тест для ReadAsync
        [Fact]
        public async Task ReadAsync_ShouldReturnElementById()
        {
            var animal = Animal.CreateNew();
            await _service.CreateAsync(animal); 

            var result = await _service.ReadAsync(animal.Id);

            Assert.NotNull(result);
            Assert.Equal(animal.Id, result?.Id);
        }

        // Тест для UpdateAsync
        [Fact]
        public async Task UpdateAsync_ShouldUpdateElement()
        {
            var animal = Animal.CreateNew();
            await _service.CreateAsync(animal); 

            animal.Age = 10;
            var result = await _service.UpdateAsync(animal);

            Assert.True(result);

            var updatedAnimal = await _service.ReadAsync(animal.Id);
            Assert.Equal(10, updatedAnimal?.Age); 
        }
    }
}
