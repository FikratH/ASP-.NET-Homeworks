using Microsoft.EntityFrameworkCore;
using Pronia.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options=>
{
    options.UseSqlServer("Server=PAVILION-FIKRET\\MSSQLSERVER02;Database=ProniaDB;Trusted_Connection=True");
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
