using Animals.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Animals.Infrastructure
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public CrudServiceAsync(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            return true;
        }

        public async Task<T> ReadAsync(object id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var all = await _repository.GetAllAsync();
            return all is null
                ? new List<T>()
                : new List<T>(all).Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.Update(element);
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.Delete(element);
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            if (_repository is AnimalsContext context)
            {
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
