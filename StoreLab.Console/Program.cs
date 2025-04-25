using System.Collections.Concurrent;
using System.Threading;

var service = new CrudServiceAsync<Product>(p => p.Id, "products.json");
var products = new ConcurrentBag<Product>();

object lockObj = new();
Semaphore semaphore = new(2, 2);
AutoResetEvent resetEvent = new(false);

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Створення 1000 товарів паралельно...");

Parallel.For(0, 1000, i =>
{
    semaphore.WaitOne();
    var product = Product.CreateNew();
    products.Add(product);
    service.CreateAsync(product).Wait();
    lock (lockObj)
    {
        if (i == 500) resetEvent.Set();
    }
    semaphore.Release();
});

resetEvent.WaitOne();
Console.WriteLine("Досягнуто 500 елементів — виконується аналіз...");

var prices = products.Select(p => p.Price).ToList();
var quantities = products.Select(p => p.Quantity).ToList();

Console.WriteLine($"\nMin Price: {prices.Min():C2}");
Console.WriteLine($"Max Price: {prices.Max():C2}");
Console.WriteLine($"Avg Price: {prices.Average():C2}");

Console.WriteLine($"\nMin Quantity: {quantities.Min()}");
Console.WriteLine($"Max Quantity: {quantities.Max()}");
Console.WriteLine($"Avg Quantity: {quantities.Average():F2}");

await service.SaveAsync();
Console.WriteLine("\nКолекцію збережено у файл products.json");