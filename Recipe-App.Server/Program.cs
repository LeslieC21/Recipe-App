using Microsoft.EntityFrameworkCore;
using Recipe_App.Server.Data;
using Recipe_App.Server.Services;
using Scalar.AspNetCore;
using SixLabors.ImageSharp.Web.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CONNECTION STRING
builder.Services.AddDbContext<RecipeDatabaseContext>(
    DbContextOptions => DbContextOptions.UseSqlServer(
            builder.Configuration.GetConnectionString("LocalConnection"))
    );

builder.Services.AddScoped<IRecipeService, RecipeService>();

builder.Services.AddImageSharp();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

app.UseImageSharp();
app.UseStaticFiles();