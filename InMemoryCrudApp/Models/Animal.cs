using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryCrudApp.Models
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public static Animal CreateNew()
        {
            var random = new Random();
            var names = new[] { "Лев", "Тигр", "Слон", "Жираф", "Кенгуру", "Ведмідь", "Панда", "Вовк", "Кіт", "Пес" };

            return new Animal
            {
                Id = Guid.NewGuid(),
                Name = names[random.Next(names.Length)],
                Age = random.Next(1, 20)
            };
        }
    }
}
