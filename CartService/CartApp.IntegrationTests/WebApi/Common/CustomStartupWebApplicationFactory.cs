using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CartApp.IntegrationTests.WebApi.Common;

/// <inheritdoc/>
public class CustomStartupWebApplicationFactory<TTestStartup, TStartup> : WebApplicationFactory<TTestStartup>
    where TTestStartup : class, TStartup
    where TStartup : class
{
    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        builder.UseContentRoot(projectDir);

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.SetBasePath(projectDir);
            configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        });

        base.ConfigureWebHost(builder);
    }

    /// <inheritdoc/>
    protected override IHostBuilder CreateHostBuilder()
        => Host.CreateDefaultBuilder(null)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<TTestStartup>();
            });
}