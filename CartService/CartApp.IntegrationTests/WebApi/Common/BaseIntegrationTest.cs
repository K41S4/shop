using LiteDB.Async;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CartApp.IntegrationTests.WebApi.Common
{
    /// <summary>
    /// Base class for integration tests.
    /// </summary>
    public abstract class BaseIntegrationTest
        : IClassFixture<CustomStartupWebApplicationFactory<FakeStartup, Startup>>, IDisposable
    {
        private readonly IServiceScope scope;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseIntegrationTest"/> class.
        /// Configuring services and creating test client for integration test cases.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="fixture">The fixture.</param>
        protected BaseIntegrationTest(CustomStartupWebApplicationFactory<FakeStartup, Startup> factory, LiteDbFixture fixture)
        {
            this.Factory = factory;
            this.LiteDbFixture = fixture;
            this.scope = this.Factory.Services.CreateScope();
            this.DbContext = this.scope.ServiceProvider.GetRequiredService<ILiteDatabaseAsync>();
            this.HttpClient = this.Factory.CreateClient();
        }

        /// <summary>
        /// Gets or sets the HTTP client used for making requests in integration tests.
        /// </summary>
        protected HttpClient HttpClient { get; set; }

        /// <summary>
        /// Gets or sets the database context used for accessing the database in integration tests.
        /// </summary>
        protected ILiteDatabaseAsync DbContext { get; set; }

        /// <summary>
        /// Gets the factory used for creating the test server and client.
        /// </summary>
        protected WebApplicationFactory<FakeStartup> Factory { get; }

        /// <summary>
        /// Gets the LiteDB fixture for managing the test database.
        /// </summary>
        protected LiteDbFixture LiteDbFixture { get; }

        /// <summary>
        /// Disposes the test resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the test resources.
        /// </summary>
        /// <param name="disposing">Whether disposing is in progress.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.scope.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
