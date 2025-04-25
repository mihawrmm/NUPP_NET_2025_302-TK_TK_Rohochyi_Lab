using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using AsyncLab.Common;

var service = new CrudServiceAsync<Bus>(b => b.Id, "buses.json");
var buses = new ConcurrentBag<Bus>();

object lockObj = new();
Semaphore semaphore = new(2, 2);
AutoResetEvent resetEvent = new(false);

Console.WriteLine("Створення 1000 автобусів паралельно...");
Parallel.For(0, 1000, i =>
{
    semaphore.WaitOne();
    var bus = Bus.CreateNew();
    buses.Add(bus);
    service.CreateAsync(bus).Wait();
    lock (lockObj)
    {
        if (i == 500) resetEvent.Set();
    }
    semaphore.Release();
});

resetEvent.WaitOne();
Console.WriteLine("Досягнуто 500 елементів — продовжуємо обчислення");

var capacities = buses.Select(b => b.Capacity).ToList();
var fuel = buses.Select(b => b.FuelConsumption).ToList();

Console.WriteLine($"\nMin Capacity: {capacities.Min()}");
Console.WriteLine($"Max Capacity: {capacities.Max()}");
Console.WriteLine($"Avg Capacity: {capacities.Average():F2}");

Console.WriteLine($"\nMin Fuel: {fuel.Min():F2}");
Console.WriteLine($"Max Fuel: {fuel.Max():F2}");
Console.WriteLine($"Avg Fuel: {fuel.Average():F2}");

await service.SaveAsync();
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("\nКолекцію збережено у файл buses.json");