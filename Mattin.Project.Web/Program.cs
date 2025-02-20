using Mattin.Project.Infrastructure;
using Mattin.Project.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add infrastructure services (this includes database configuration)
builder.Services.AddInfrastructure();

var app = builder.Build();

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Add additional routes for category and search
app.MapControllerRoute(
    name: "serviceCategory",
    pattern: "Service/Category/{category}",
    defaults: new { controller = "Service", action = "Category" }
);

app.MapControllerRoute(
    name: "clientSearch",
    pattern: "Client/Search/{name?}",
    defaults: new { controller = "Client", action = "Search" }
);

app.Run();
