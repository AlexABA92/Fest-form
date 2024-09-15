using Fest_form.data;
using Microsoft.EntityFrameworkCore;
using testBD.services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("FestDb2025"),
       new MySqlServerVersion(new Version(8, 0, 39))
    )
);
builder.Services.AddSingleton<SshTunnelServices>();
builder.Services.AddControllersWithViews();


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var shhTunnelService = services.GetRequiredService<SshTunnelServices>();
    await shhTunnelService.InitializeSshTunnel(services);

    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();
}

app.Run();
