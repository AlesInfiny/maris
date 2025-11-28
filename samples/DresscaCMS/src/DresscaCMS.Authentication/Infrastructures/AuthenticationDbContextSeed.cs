using DresscaCMS.Authentication.Infrastructures.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DresscaCMS.Authentication.Infrastructures;

public static class AuthenticationDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        // スコープを作成
        using var scope = services.CreateScope();
        var scopedProvider = scope.ServiceProvider;

        var context = scopedProvider.GetRequiredService<AuthenticationDbContext>();
        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // DB マイグレーションを適用
        await context.Database.EnsureCreatedAsync();

        // --- 初期ユーザー作成例 ---
        const string adminEmail = "user@example.com";
        const string initialPassword = "P@ssw0rd1"; // 本番では必ず変更してもらう

        // すでに作成済みかチェック
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
                // ログ出力などに差し替えてもOK
                throw new InvalidOperationException(
                    $"Failed to create initial user: {string.Join(", ", result.Errors)}");
            }
        }
    }
}
