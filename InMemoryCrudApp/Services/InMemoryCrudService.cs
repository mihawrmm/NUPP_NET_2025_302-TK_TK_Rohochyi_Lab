using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InMemoryCrudApp.Services
{
    public class InMemoryCrudService<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly ConcurrentDictionary<Guid, T> _items = new();
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public InMemoryCrudService(string filePath)
        {
            _filePath = filePath;
            LoadFromFileAsync().Wait();
        }

        public async Task<bool> CreateAsync(T element)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
                throw new Exception("Клас повинен мати властивість Id типу Guid");

            var id = (Guid)idProperty.GetValue(element);

            bool added = _items.TryAdd(id, element);
            return await Task.FromResult(added);
        }

        public async Task<T> ReadAsync(Guid id)
        {
            _items.TryGetValue(id, out var element);
            return await Task.FromResult(element);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await Task.FromResult(_items.Values.ToList());
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            return await Task.FromResult(_items.Values.Skip((page - 1) * amount).Take(amount).ToList());
        }

        public async Task<bool> UpdateAsync(T element)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
                throw new Exception("Клас повинен мати властивість Id типу Guid");

            var id = (Guid)idProperty.GetValue(element);

            if (!_items.ContainsKey(id))
                return false;

            _items[id] = element;
            return await Task.FromResult(true);
        }

        public async Task<bool> RemoveAsync(T element)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
                throw new Exception("Клас повинен мати властивість Id типу Guid");

            var id = (Guid)idProperty.GetValue(element);

            bool removed = _items.TryRemove(id, out _);
            return await Task.FromResult(removed);
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                await _semaphore.WaitAsync();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var json = JsonSerializer.Serialize(_items.Values, options);
                await File.WriteAllTextAsync(_filePath, json);

                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task LoadFromFileAsync()
        {
            if (!File.Exists(_filePath))
                return;

            try
            {
                await _semaphore.WaitAsync();

                var json = await File.ReadAllTextAsync(_filePath);
                var items = JsonSerializer.Deserialize<List<T>>(json);

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var idProperty = typeof(T).GetProperty("Id");
                        if (idProperty != null)
                        {
                            var id = (Guid)idProperty.GetValue(item);
                            _items.TryAdd(id, item);
                        }
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
