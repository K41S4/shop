using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.IntegrationTests.WebApi;

/// <inheritdoc/>
public class CustomStartupWebApplicationFactory<TTestStartup, TStartup> : WebApplicationFactory<TTestStartup>
    where TTestStartup : class, TStartup
    where TStartup : class
{
    /// <inheritdoc/>
    protected override IHostBuilder CreateHostBuilder()
        => Host.CreateDefaultBuilder(null)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<TTestStartup>();
            });
}