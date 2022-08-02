using Infrastructure.IdentityConfigs;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ConnectionStrings
var connectionString = builder.Configuration["ConnectionString:SqlDbConnection"];
builder.Services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(connectionString));

builder.Services.AddIdentityConfig(builder.Configuration);

builder.Services.AddAuthorization();
// Coockie Configs
builder.Services.ConfigureApplicationCookie(option => {
    option.LoginPath = "/Account/Login";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(240);
    option.AccessDeniedPath = "/Account/AccessDenied";
    option.SlidingExpiration = true;
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
