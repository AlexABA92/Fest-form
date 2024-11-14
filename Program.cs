using Fest_form.data;
using Fest_form.data.Entity;
using Fest_form.Interface;
using Fest_form.Repositories;
using Fest_form.Repositories.DeanseTeamRepos;
using Fest_form.Repositories.FileRepos;
using Fest_form.Services;
using Fest_form.Services.Bucket;
using Fest_form.Services.MailSend;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;


//using Serilog;
//using Serilog.Events;

using testBD.services;

using static System.Runtime.InteropServices.JavaScript.JSType;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddAzureWebAppDiagnostics();


builder.Services.Configure<AzureBlobLoggerOptions>(options =>
{
    options.BlobName = "log.txt";
});

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
builder.Services.AddScoped<IDanceTeamRepos<DanceTeam>,DanceTeamRepos>();
builder.Services.AddScoped<IBucket, Bucket>();
builder.Services.AddScoped<IMailSend, MailSend>();
builder.Services.AddScoped<IFileRepos,FilesRepos>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


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



