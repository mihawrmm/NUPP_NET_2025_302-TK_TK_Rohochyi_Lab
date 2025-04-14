using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Common
{
    public class Enclosure
    {
        // Статичне поле
        public static int TotalEnclosures;

        // Статичний конструктор
        static Enclosure()
        {
            TotalEnclosures = 0;
        }

        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool HasWaterSource { get; set; }

        // Делегат і подія
        public delegate void AnimalEnteredHandler(string enclosureName);
        public event AnimalEnteredHandler OnAnimalEnter;

        // Конструктор
        public Enclosure(string name, int capacity, bool hasWater)
        {
            Name = name;
            Capacity = capacity;
            HasWaterSource = hasWater;
            TotalEnclosures++;
        }

        // Метод
        public void EnterAnimal(string animalName)
        {
            Console.WriteLine($"{animalName} увійшов до вольєру {Name}.");
            OnAnimalEnter?.Invoke(Name);
        }

        // Статичний метод
        public static void ShowTotalEnclosures()
        {
            Console.WriteLine($"Всього вольєрів: {TotalEnclosures}");
        }
    }

    // Метод розширення
    public static class EnclosureExtensions
    {
        public static string GetSummary(this Enclosure enclosure)
        {
            return $"{enclosure.Name}: місткість {enclosure.Capacity}, вода: {(enclosure.HasWaterSource ? "є" : "немає")}";
        }
    }
}
