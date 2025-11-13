using LiteDB.Async;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartApp.IntegrationTests.WebApi.Common
{
    /// <summary>
    /// Fake startup class for integration tests.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    public class FakeStartup(IConfiguration configuration) : Startup(configuration)
    {
        /// <inheritdoc />
        protected override void ConfigureDbContext(IServiceCollection services)
        {
            services.AddSingleton<ILiteDatabaseAsync>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("CartDB");
                return new LiteDatabaseAsync(connectionString);
            });
        }
    }
}
