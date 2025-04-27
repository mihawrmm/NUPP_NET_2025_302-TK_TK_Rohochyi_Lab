using InMemoryCrudApp.Models;
using InMemoryCrudApp.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var service = new InMemoryCrudService<Animal>("animals.json");

Console.WriteLine("Починається створення 1000 тварин...");

Parallel.For(0, 1000, async i =>
{
    var animal = Animal.CreateNew();
    await service.CreateAsync(animal);
});

await service.SaveAsync();

Console.WriteLine("Створено 1000 тварин та збережено у файл 'animals.json'!");

var animals = await service.ReadAllAsync();

var minAge = animals.Min(a => a.Age);
var maxAge = animals.Max(a => a.Age);
var avgAge = animals.Average(a => a.Age);

Console.WriteLine($"\nСтатистика віку тварин:");
Console.WriteLine($"- Мінімальний вік: {minAge}");
Console.WriteLine($"- Максимальний вік: {maxAge}");
Console.WriteLine($"- Середній вік: {avgAge:F2}");
Console.WriteLine($"Файл буде збережено за шляхом: {Path.GetFullPath("animals.json")}");

Console.WriteLine("\nДемонстрація використання lock:");

int counter = 0;
object counterLock = new object();

Parallel.For(0, 1000, i =>
{
    lock (counterLock)
    {
        counter++;
    }
});

Console.WriteLine($"Лічильник lock: {counter}");


Console.WriteLine("\nДемонстрація використання SemaphoreSlim:");

SemaphoreSlim semaphore = new SemaphoreSlim(3); 
List<Task> tasks = new List<Task>();

for (int i = 0; i < 10; i++)
{
    int taskId = i;
    tasks.Add(Task.Run(async () =>
    {
        await semaphore.WaitAsync();
        try
        {
            Console.WriteLine($"Потік {taskId} увійшов у критичну секцію.");
            await Task.Delay(500); 
        }
        finally
        {
            Console.WriteLine($"Потік {taskId} виходить із критичної секції.");
            semaphore.Release();
        }
    }));
}

await Task.WhenAll(tasks);

Console.WriteLine("\nДемонстрація використання AutoResetEvent:");

AutoResetEvent autoEvent = new AutoResetEvent(true);

void Worker(int id)
{
    autoEvent.WaitOne(); 

    Console.WriteLine($"Потік {id} почав роботу.");
    Thread.Sleep(500); 
    Console.WriteLine($"Потік {id} завершив роботу.");

    autoEvent.Set(); 
}

List<Task> autoResetTasks = new List<Task>();

for (int i = 0; i < 5; i++)
{
    int workerId = i;
    autoResetTasks.Add(Task.Run(() => Worker(workerId)));
}

await Task.WhenAll(autoResetTasks);

Console.WriteLine("\nРобота завершена! Натисни будь-яку клавішу...");
Console.ReadKey();
