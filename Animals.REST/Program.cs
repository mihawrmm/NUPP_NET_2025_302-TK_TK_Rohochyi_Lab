using Animals.Infrastructure.Repositories;
using Animals.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();      
builder.Services.AddSwaggerGen();                

builder.Services.AddDbContext<AnimalsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

var app = builder.Build();

app.UseSwagger();            
app.UseSwaggerUI();           

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
