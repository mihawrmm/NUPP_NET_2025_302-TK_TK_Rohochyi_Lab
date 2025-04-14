using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Common
{
    public class Mammal : Animal
    {
        // Статичне поле
        public static int MammalCount;

        // Статичний конструктор
        static Mammal()
        {
            MammalCount = 0;
        }

        public bool IsPredator { get; set; }
        public string FurType { get; set; }
        public int DailyFoodKg { get; set; }

        // Делегат і подія
        public delegate void FeedMammalHandler(Mammal m);
        public event FeedMammalHandler OnFed;

        // Конструктор
        public Mammal(string name, int age, string origin, bool isPredator, string furType, int foodKg)
            : base(name, age, origin)
        {
            IsPredator = isPredator;
            FurType = furType;
            DailyFoodKg = foodKg;
            MammalCount++;
        }

        // Метод
        public override void Speak()
        {
            Console.WriteLine($"{Name} гарчить.");
        }

        public void Feed()
        {
            Console.WriteLine($"{Name} був нагодований.");
            OnFed?.Invoke(this);
        }

        // Статичний метод
        public static void ShowMammalsCount()
        {
            Console.WriteLine($"Ссавців у зоопарку: {MammalCount}");
        }
    }
}
