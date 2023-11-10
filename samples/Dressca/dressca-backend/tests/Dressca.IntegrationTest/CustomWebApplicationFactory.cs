﻿using Dressca.EfInfrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Dressca.IntegrationTest;
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? Environments.Development;

        if (!env.Equals(Environments.Development, StringComparison.OrdinalIgnoreCase))
        {
            builder.ConfigureServices(services =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .Build();

                services.AddDresscaEfInfrastructure(config);
            });
        }
    }
}
