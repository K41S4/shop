using Catalog.Persistence.DBContext;
using Catalog.WebAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IntegrationTests.WebApi
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
            services.AddDbContext<CatalogDBContext>(options =>
                options.UseInMemoryDatabase("InMemoryCatalogTestDb"));
        }
    }
}
