using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.Common;

namespace ZooCons
{
    public class Program
    {
        static void Main(string[] args)
        {
            var animalService = new InMemoryCrudService<Animal>();

            //  Додамо тварин
            animalService.Create(new Animal("Слон", 10, "Індія"));
            animalService.Create(new Animal("Тигр", 5, "Бангладеш"));

            //  Зберігаємо в файл
            string filePath = "animals.json";
            animalService.Save(filePath);
            Console.WriteLine("✅ Тварини збережено у файл.");

            // Очищаємо список
            animalService.Delete(a => true);

            Console.WriteLine("❌ Тварини видалені з памʼяті.");
            Console.WriteLine("📋 Кількість зараз: " + animalService.ReadAll().Count());

            // Завантаження з файлу
            animalService.Load(filePath);
            Console.WriteLine("✅ Тварини завантажені з файлу:");
            foreach (var animal in animalService.ReadAll())
            {
                Console.WriteLine(animal.GetInfo());
            }
        }
    }
}

