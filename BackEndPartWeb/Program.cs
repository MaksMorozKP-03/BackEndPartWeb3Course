using BackEndPartWeb.Data;
using BackEndPartWeb.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<BestiaryContext>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IMonstersService, MonstersService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IClasificationsService, ClasificationsService>();
DbInitializer.Initialize(new BestiaryContext(new DbContextOptions<BestiaryContext>()));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();
