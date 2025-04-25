using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;

public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
{
    private readonly ConcurrentDictionary<Guid, T> _storage = new();
    private readonly Func<T, Guid> _getIdFunc;
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public CrudServiceAsync(Func<T, Guid> getIdFunc, string filePath)
    {
        _getIdFunc = getIdFunc;
        _filePath = filePath;
    }

    public async Task<bool> CreateAsync(T element)
    {
        var id = _getIdFunc(element);
        return _storage.TryAdd(id, element);
    }

    public async Task<T?> ReadAsync(Guid id)
    {
        _storage.TryGetValue(id, out var result);
        return result;
    }

    public async Task<IEnumerable<T>> ReadAllAsync() => _storage.Values;

    public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount) =>
        _storage.Values.Skip((page - 1) * amount).Take(amount);

    public async Task<bool> UpdateAsync(T element)
    {
        var id = _getIdFunc(element);
        _storage[id] = element;
        return true;
    }

    public async Task<bool> RemoveAsync(T element)
    {
        var id = _getIdFunc(element);
        return _storage.TryRemove(id, out _);
    }

    public async Task<bool> SaveAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(_storage.Values);
            await File.WriteAllTextAsync(_filePath, json);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public IEnumerator<T> GetEnumerator() => _storage.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}