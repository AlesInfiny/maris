using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dressca.IntegrationTest;

public class ApiTestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var config = this.GetConfiguration();

            // テスト用のJwtBearer認証の設定追加
            // デフォルトで認証に使用されるAuthenticationScheme名を"Test"に設定
            services.AddAuthentication("Test")
            .AddJwtBearer("Test", options =>
            {
                // JWTの検証内容を設定
                options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new NullReferenceException("Jwt:Key"))),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
        });
    }

    internal string CreateToken(string userName)
    {
        // JWTの生成
        var config = this.GetConfiguration();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new NullReferenceException("Jwt:Key")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    internal IConfiguration GetConfiguration()
    {
        // テスト用のappsettings.jsonを追加(JWT認証で利用する設定値を記載)
        return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .Build();
    }
}
