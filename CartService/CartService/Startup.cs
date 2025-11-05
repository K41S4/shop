using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CartApp.BusinessLogic.Services;
using CartApp.Persistence.Repositories;
using CartApp.WebApi.Controllers.v1;
using CartApp.WebApi.Filters;
using CartApp.WebApi.MappingProfiles;
using LiteDB.Async;
using Mapster;
using MapsterMapper;

namespace CartApp;

/// <summary>
/// Startup class for configuring services and middleware.
/// </summary>
public class Startup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Configures services for the application.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        })
            .AddApplicationPart(typeof(CartsV1Controller).Assembly);

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICartRepository, CartRepository>();
        this.ConfigureDbContext(services);

        var config = new TypeAdapterConfig();
        config.Scan(typeof(CartItemMappingConfig).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <param name="env">Web host environment.</param>
    /// <param name="provider">Api version description provider.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(cfg =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    cfg.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    /// <summary>
    /// Configures the database context.
    /// </summary>
    /// <param name="services">Service collection.</param>
    protected virtual void ConfigureDbContext(IServiceCollection services)
    {
        services.AddScoped<ILiteDatabaseAsync>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("CartDB");
            return new LiteDatabaseAsync(connectionString);
        });
    }
}