using Microsoft.EntityFrameworkCore;
using vlf_4rum.Data;
using VlfForum.Extensions;
using VlfForum.Middleware;
using VlfForum.Services.Implementations;
using VlfForum.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseGlobalException();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

/* AUTO MIGRATE */

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();