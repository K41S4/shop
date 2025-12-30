namespace APIGateway;

/// <summary>
/// Program class.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main method.
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
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
                if (env.IsDevelopment())
                {
                    config.AddJsonFile($"ocelot.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}