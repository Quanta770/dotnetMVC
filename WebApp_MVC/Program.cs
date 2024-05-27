using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository;
using WebApp.DataAccess.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//tell application to use Entity Framwork Core
builder.Services.AddDbContext<ApplicationDBContext>(
    options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    ));
//add interface for dependency injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.WebHost.UseStaticWebAssets();   

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
