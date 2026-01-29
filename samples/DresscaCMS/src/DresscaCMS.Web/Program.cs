using DresscaCMS.Announcement;
using DresscaCMS.Authentication;
using DresscaCMS.Authentication.Infrastructures;
using DresscaCMS.Web.Components;
using DresscaCMS.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Primitives;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

var maxRequestBodySizeBytes = builder.Configuration.GetValue<long?>("MaxRequestBodySizeBytes");

if (maxRequestBodySizeBytes.HasValue)
{
    // Kestrel サーバーのリクエストボディサイズの上限を設定
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Limits.MaxRequestBodySize = maxRequestBodySizeBytes;
    });

    // フォームオプションのリクエストボディサイズの上限を設定
    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = maxRequestBodySizeBytes.Value;
        options.BufferBodyLengthLimit = maxRequestBodySizeBytes.Value;
    });
}

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

// 入れ子になったオブジェクトのバリデーションをサポートするためのサービスを登録
builder.Services.AddValidation();

if (builder.Environment.IsDevelopment())
{
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

app.Use((context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    const string cspHeaderName = "Content-Security-Policy";
    const string desiredCsp = "frame-ancestors 'none'";

    // ASP.NET Core の Response.Headers は同名ヘッダーを複数値として保持しうるため、
    // ここで「既に存在する CSP 値」を観測してログに出します。
    if (context.Response.Headers.TryGetValue(cspHeaderName, out StringValues existingCspValues)
        && !StringValues.IsNullOrEmpty(existingCspValues))
    {
        logger.LogWarning(
            "{HeaderName} is already set before security-header middleware. Values: {HeaderValues}. Path: {Path}",
            cspHeaderName,
            existingCspValues.ToArray(),
            context.Request.Path.Value);

        // 追記せずに 1 本化する（既存値を取り除いてから設定）
        context.Response.Headers.Remove(cspHeaderName);
    }

    // コンテンツタイプを誤認識しないよう、HTTPレスポンスヘッダに「X-Content-Type-Options: nosniff」の設定を追加
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";

    // レガシーブラウザー向けのクリックジャッキング攻撃への対策として、HTTP レスポンスヘッダに、「X-FRAME-OPTIONS」を「DENY」に設定
    context.Response.Headers["X-Frame-Options"] = "DENY";

    // クリックジャッキング攻撃への対策として、 CSP frame-ancestors を設定
    // context.Response.Headers["Content-Security-Policy"] = "frame-ancestors 'none'";
    context.Response.Headers[cspHeaderName] = desiredCsp;

    // レスポンス開始時点の「最終的な CSP ヘッダー」をログに出す（重複があればここで分かる）
    context.Response.OnStarting(() =>
    {
        if (context.Response.Headers.TryGetValue(cspHeaderName, out var cspValues))
        {
            logger.LogInformation(
                "{HeaderName} at response start: {HeaderValues}. Path: {Path}",
                cspHeaderName,
                cspValues.ToArray(),
                context.Request.Path.Value);
        }
        else
        {
            logger.LogInformation(
                "{HeaderName} at response start: <missing>. Path: {Path}",
                cspHeaderName,
                context.Request.Path.Value);
        }

        return Task.CompletedTask;
    });

    return next();
});

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
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
