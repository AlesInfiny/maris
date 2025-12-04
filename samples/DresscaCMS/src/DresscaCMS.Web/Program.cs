using DresscaCMS.Announcement.ApplicationCore.ApplicationServices;
using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using DresscaCMS.Announcement.Infrastructures;
using DresscaCMS.Web.Components;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddRazorPages();

builder.Services.AddDbContextFactory<AnnouncementDbContext>(options =>
{
    options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dressca.Cms.Announcement;Integrated Security=True", providerOptions =>
    {
        providerOptions.EnableRetryOnFailure();
    });
});

builder.Services.AddScoped<AnnouncementsApplicationService>();
builder.Services.AddScoped<IAnnouncementsRepository, EfAnnouncementsRepository>();

if (builder.Environment.IsDevelopment())
{
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ServerError", createScopeForErrors: true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorPages();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
