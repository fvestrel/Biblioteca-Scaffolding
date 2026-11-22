using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

var cnBuilder = new MySqlConnectionStringBuilder(builder.Configuration.GetConnectionString("cn"));
var pwd = builder.Configuration["MySql:Password"];
if (!string.IsNullOrEmpty(pwd))
{
    cnBuilder.Password = pwd;
}
builder.Services.AddDbContext<BibliotecaDbContext>(opciones => opciones.UseMySQL(cnBuilder.ConnectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
