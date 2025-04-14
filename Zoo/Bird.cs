using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Common
{
    public class Bird : Animal
    {
        // Статичне поле
        public static int BirdCount;

        // Статичний конструктор
        static Bird()
        {
            BirdCount = 0;
        }

        public double Wingspan { get; set; }
        public bool CanFly { get; set; }
        public string BeakType { get; set; }

        // Делегат і подія
        public delegate void BirdFlewHandler(Bird b);
        public event BirdFlewHandler OnFly;

        // Конструктор
        public Bird(string name, int age, string origin, double wingspan, bool canFly, string beakType)
            : base(name, age, origin)
        {
            Wingspan = wingspan;
            CanFly = canFly;
            BeakType = beakType;
            BirdCount++;
        }

        // Метод
        public override void Speak()
        {
            Console.WriteLine($"{Name} щебече.");
        }

        public void Fly()
        {
            if (CanFly)
            {
                Console.WriteLine($"{Name} летить.");
                OnFly?.Invoke(this);
            }
            else
            {
                Console.WriteLine($"{Name} не вміє літати.");
            }
        }

        // Статичний метод
        public static void ShowBirdCount()
        {
            Console.WriteLine($"Птахів у зоопарку: {BirdCount}");
        }
    }
}
