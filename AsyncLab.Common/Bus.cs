using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncLab.Common
{
    public class Bus
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        public string Route { get; set; } = string.Empty;
        public double FuelConsumption { get; set; }

        public static Bus CreateNew()
        {
            var rand = new Random();
            return new Bus
            {
                Id = Guid.NewGuid(),
                Capacity = rand.Next(10, 100),
                Route = "Route " + rand.Next(1, 100),
                FuelConsumption = rand.NextDouble() * 30 + 5
            };
        }
    }
}