using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Zoo.Common
{
    public class InMemoryCrudService<T> : ICrudService<T>
    {
        private readonly List<T> _items = new List<T>();

        public void Create(T item) => _items.Add(item);
        public T Read(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);
        public IEnumerable<T> ReadAll() => _items.ToList();
        public void Update(Func<T, bool> predicate, T newItem)
        {
            var index = _items.FindIndex(x => predicate(x));
            if (index >= 0)
                _items[index] = newItem;
        }
        public void Delete(Func<T, bool> predicate) => _items.RemoveAll(x => predicate(x));

        // 📥 Збереження у файл
        public void Save(string filePath)
        {
            var json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var loadedItems = JsonConvert.DeserializeObject<List<T>>(json);
                if (loadedItems != null)
                {
                    _items.Clear();
                    _items.AddRange(loadedItems);
                }
            }
        }
    }
}
