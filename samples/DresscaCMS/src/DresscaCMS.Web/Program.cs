using DresscaCMS.Announcement;
using DresscaCMS.Authentication;
using DresscaCMS.Authentication.Infrastructures;
using DresscaCMS.Web.Components;
using DresscaCMS.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddRazorPages();
builder.Services.AddInMemoryStateStore();

// お知らせメッセージに関するサービス一式を登録
builder.Services.AddAnnouncementsServices(
    builder.Configuration,
    builder.Environment);

// 認証に関するサービス一式を登録
builder.Services.AddAuthenticationServices(
    builder.Configuration,
    builder.Environment);

if (builder.Environment.IsDevelopment())
{
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
    builder.Services.AddHttpLogging(logging =>
    {
        // どのデータをどのくらいの量出力するか設定。
        // 適宜設定値は変更する。
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });
}

// Blazor に依存した認証に関するサービスを登録
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // HTTP 通信ログを有効にする。
    app.UseHttpLogging();
    await AuthenticationDbContextSeed.SeedAsync(app.Services);
}

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

app.MapAdditionalIdentityEndpoints();

app.Run();
