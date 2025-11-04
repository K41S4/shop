using Microsoft.Extensions.Configuration;

namespace CartApp.IntegrationTests.WebApi.Common
{
    /// <summary>
    /// Fixture for LiteDB database.
    /// </summary>
    public class LiteDbFixture : IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteDbFixture"/> class.
        /// </summary>
        public LiteDbFixture()
        {
            var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            this.DatabaseFilePath = config.GetConnectionString("CartDB")!;
        }

        /// <summary>
        /// Gets the database file path.
        /// </summary>
        public string DatabaseFilePath { get; }

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        /// <param name="disposing">Whether disposing is in progress.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing && File.Exists(this.DatabaseFilePath))
                {
                    File.Delete(this.DatabaseFilePath);
                }

                this.disposed = true;
            }
        }
    }
}
