using Animals.Infrastructure.Repositories;
using Animals.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Animals.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

//  JWT аутентифікація
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "ZooApi",
            ValidAudience = "ZooApiClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_secret_zoo_key_12345"))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Animals API", Version = "v1" });

    // JWT авторизація
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введіть токен у форматі: Bearer {токен}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddDbContext<AnimalsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AnimalsContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

var app = builder.Build();

app.UseSwagger();            
app.UseSwaggerUI();           

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
async Task CreateRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = new[] { "User", "Keeper", "Admin" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}
await CreateRolesAsync(app);
async Task AssignRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    var keeper = await userManager.FindByNameAsync("keeper1");
    if (keeper != null)
        await userManager.AddToRoleAsync(keeper, "Keeper");

    var admin = await userManager.FindByNameAsync("admin1");
    if (admin != null)
        await userManager.AddToRoleAsync(admin, "Admin");
}

await AssignRolesAsync(app);

app.Run();
