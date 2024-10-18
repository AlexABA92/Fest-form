using Fest_form.data;
using Fest_form.data.Entity;
using Fest_form.Interface;
using Fest_form.Repositories;

using Microsoft.EntityFrameworkCore;
using testBD.services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FestDataContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("FestDb2025"),
       new MySqlServerVersion(new Version(8, 0, 39))
    )
);
builder.Services.AddSingleton<SshTunnelServices>();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.Name = "session";
});
builder.Services.AddScoped<IGenreRepos<Genre>, GenreRepos>();
builder.Services.AddScoped<ICategory<Category>, CategoryRepos>();
builder.Services.AddScoped<IParticipantsNumber<ParticipantsNumber>, ParticipantsRepos>();

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

//var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
//app.UseRequestLocalization(localizationOptions.Value);

app.UseSession();
app.UseRouting();

app.Use(async (context, next) => {
    string cookie = string.Empty;
    if(context.Request.Cookies.TryGetValue("Language",out cookie))
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie);
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie);
    }
    else
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("uk");
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("uk");
    }
    await next.Invoke();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Registration}/{action=Index}/{Id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var shhTunnelService = services.GetRequiredService<SshTunnelServices>();
    await shhTunnelService.InitializeSshTunnel(services);

    var context = services.GetRequiredService<FestDataContext>();
    context.Database.Migrate();
}

app.Run();
