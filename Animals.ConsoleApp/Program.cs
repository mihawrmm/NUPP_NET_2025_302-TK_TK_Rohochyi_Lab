using Animals.Infrastructure;
using Animals.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddDbContext<AnimalsContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

var serviceProvider = services.BuildServiceProvider();

var animalService = serviceProvider.GetRequiredService<ICrudServiceAsync<Animals.Infrastructure.Models.AnimalModel>>();

Console.WriteLine("Твоя програма успішно підключилася до бази даних через CRUD-сервіс!")

while (true)
{
    Console.WriteLine("\nЩо бажаєте зробити?");
    Console.WriteLine("1 - Додати тварину");
    Console.WriteLine("2 - Переглянути всіх тварин");
    Console.WriteLine("3 - Знайти тварину за ID");
    Console.WriteLine("4 - Оновити тварину");
    Console.WriteLine("5 - Видалити тварину");
    Console.WriteLine("0 - Вийти");

    var input = Console.ReadLine();

    if (input == "0")
        break;

    switch (input)
    {
        case "1":
            Console.Write("Введіть ім'я тварини: ");
            var name = Console.ReadLine();
            Console.Write("Введіть вік тварини: ");
            var age = int.Parse(Console.ReadLine());
            Console.Write("Введіть ID вольєра: ");
            var enclosureId = int.Parse(Console.ReadLine());

            await animalService.CreateAsync(new Animals.Infrastructure.Models.AnimalModel
            {
                Name = name,
                Age = age,
                EnclosureId = enclosureId
            });

            await animalService.SaveAsync();
            Console.WriteLine("Тварину додано!");
            break;

        case "2":
            var animals = await animalService.ReadAllAsync();
            foreach (var animal in animals)
            {
                Console.WriteLine($"ID: {animal.Id}, Ім'я: {animal.Name}, Вік: {animal.Age}, Вольєр ID: {animal.EnclosureId}");
            }
            break;

        case "3":
            Console.Write("Введіть ID тварини: ");
            var id = int.Parse(Console.ReadLine());
            var found = await animalService.ReadAsync(id);
            if (found != null)
            {
                Console.WriteLine($"Знайдено: Ім'я: {found.Name}, Вік: {found.Age}, Вольєр ID: {found.EnclosureId}");
            }
            else
            {
                Console.WriteLine("Тварину не знайдено.");
            }
            break;

        case "4":
            Console.Write("Введіть ID тварини для оновлення: ");
            var updateId = int.Parse(Console.ReadLine());
            var animalToUpdate = await animalService.ReadAsync(updateId);
            if (animalToUpdate != null)
            {
                Console.Write("Нове ім'я: ");
                animalToUpdate.Name = Console.ReadLine();
                Console.Write("Новий вік: ");
                animalToUpdate.Age = int.Parse(Console.ReadLine());
                Console.Write("Новий ID вольєра: ");
                animalToUpdate.EnclosureId = int.Parse(Console.ReadLine());

                await animalService.UpdateAsync(animalToUpdate);
                await animalService.SaveAsync();
                Console.WriteLine("Тварину оновлено!");
            }
            else
            {
                Console.WriteLine("Тварину не знайдено.");
            }
            break;

        case "5":
            Console.Write("Введіть ID тварини для видалення: ");
            var deleteId = int.Parse(Console.ReadLine());
            var animalToDelete = await animalService.ReadAsync(deleteId);
            if (animalToDelete != null)
            {
                await animalService.RemoveAsync(animalToDelete);
                await animalService.SaveAsync();
                Console.WriteLine("Тварину видалено!");
            }
            else
            {
                Console.WriteLine("Тварину не знайдено.");
            }
            break;

        default:
            Console.WriteLine("Невідома команда.");
            break;
    }
}

Console.WriteLine("До побачення!");

