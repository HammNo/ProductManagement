using Demo.ASP.Services;
using Microsoft.EntityFrameworkCore;
using ProductManagement.ASP.Services;
using ProductManagement.DAL;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProdManagementContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    );
builder.Services.AddScoped<SmtpClient>();
builder.Services.AddScoped<MailService>();
builder.Services.AddSingleton(builder.Configuration.GetSection("SMTP").Get<MailConfig>());
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<OrderService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
