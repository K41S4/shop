using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace CartApp
{
    /// <summary>
    /// The Program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Args.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">Args.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    var isGrpcProfile = Environment.GetEnvironmentVariable("LAUNCH_PROFILE") == "grpc";
                    if (isGrpcProfile)
                    {
                        webBuilder.UseKestrel(options =>
                        {
                            options.ListenLocalhost(9046, o => o.Protocols = HttpProtocols.Http2);
                        });
                    }
                });
    }
}
