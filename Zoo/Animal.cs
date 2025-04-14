using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Common
{
    public class Animal
    {
        // Статичне поле
        public static int AnimalCount;

        // Статичний конструктор
        static Animal()
        {
            AnimalCount = 0;
        }

        // Звичайні властивості
        public string Name { get; set; }
        public int Age { get; set; }
        public string Origin { get; set; }

        // Делегат та Подія
        public delegate void AnimalBornHandler(Animal a);
        public event AnimalBornHandler OnAnimalBorn;

        // Конструктор
        public Animal(string name, int age, string origin)
        {
            Name = name;
            Age = age;
            Origin = origin;
            AnimalCount++;

            // Виклик події
            OnAnimalBorn?.Invoke(this);
        }

        // Метод
        public virtual void Speak()
        {
            Console.WriteLine($"{Name} видає звук.");
        }

        // Статичний метод
        public static void ShowTotalAnimals()
        {
            Console.WriteLine($"У зоопарку всього {AnimalCount} тварин.");
        }
    }

    // Метод розширення
    public static class AnimalExtensions
    {
        // Метод розширення: повертає інформацію про тварину
        public static string GetInfo(this Animal animal)
        {
            return $"{animal.Name} ({animal.Origin}), {animal.Age} років";
        }
    }
}
