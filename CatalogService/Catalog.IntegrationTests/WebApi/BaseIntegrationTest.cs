using Catalog.WebAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Persistence.DBContext;

namespace Catalog.IntegrationTests.WebApi
{
    /// <summary>
    /// Base class for integration tests.
    /// </summary>
    public abstract class BaseIntegrationTest : IClassFixture<CustomStartupWebApplicationFactory<FakeStartup, Startup>>
    {
        private readonly IServiceScope scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseIntegrationTest"/> class.
        /// Configuring services and creating test client for integration test cases.
        /// </summary>
        /// <param name="factory">The factory.</param>
        protected BaseIntegrationTest(CustomStartupWebApplicationFactory<FakeStartup, Startup> factory)
        {
            this.Factory = factory;
            this.scope = this.Factory.Services.CreateScope();
            this.DbContext = this.scope.ServiceProvider.GetRequiredService<CatalogDBContext>();
            this.HttpClient = this.Factory.CreateClient();
        }

        /// <summary>
        /// Gets or sets the HTTP client used for making requests in integration tests.
        /// </summary>
        protected HttpClient HttpClient { get; set; }

        /// <summary>
        /// Gets or sets the database context used for accessing the database in integration tests.
        /// </summary>
        protected CatalogDBContext DbContext { get; set; }

        /// <summary>
        /// Gets the factory used for creating the test server and client.
        /// </summary>
        protected WebApplicationFactory<FakeStartup> Factory { get; }
    }
}
