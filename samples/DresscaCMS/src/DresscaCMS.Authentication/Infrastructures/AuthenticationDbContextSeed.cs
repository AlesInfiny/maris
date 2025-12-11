using DresscaCMS.Authentication.Infrastructures.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DresscaCMS.Authentication.Infrastructures;

/// <summary>
///  認証データベースの初期データを作成するためのシードクラスです。
/// </summary>
public static class AuthenticationDbContextSeed
{
    /// <summary>
    ///  初期ユーザーを登録します。
    /// </summary>
    /// <param name="services">依存関係の注入に使用する <see cref="IServiceProvider"/> インスタンス。</param>
    /// <returns>
    /// 非同期操作を表す <see cref="Task"/>。
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// 初期ユーザーの作成に失敗した場合にスローされます。
    /// </exception>
    public static async Task SeedAsync(IServiceProvider services)
    {
        // スコープを作成
        using var scope = services.CreateScope();
        var scopedProvider = scope.ServiceProvider;

        var context = scopedProvider.GetRequiredService<AuthenticationDbContext>();
        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // DB マイグレーションが完了していることを確認します。
        await context.Database.EnsureCreatedAsync();

        // 初期ユーザーのデータを設定します。
        const string adminEmail = "user@example.com";
        const string initialPassword = "P@ssw0rd1";

        // すでに作成済みかチェックします。
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, initialPassword);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create initial user: {string.Join(", ", result.Errors)}");
            }
        }
    }
}
