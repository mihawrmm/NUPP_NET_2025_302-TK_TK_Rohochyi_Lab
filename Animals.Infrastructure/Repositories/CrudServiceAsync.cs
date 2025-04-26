using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Animals.Infrastructure.Repositories
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly AnimalsContext _context;

        public CrudServiceAsync(IRepository<T> repository, AnimalsContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            return true;
        }

        public async Task<T> ReadAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            return await _context.Set<T>()
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await Task.Run(() => _repository.Update(element));
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await Task.Run(() => _repository.Delete(element));
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
