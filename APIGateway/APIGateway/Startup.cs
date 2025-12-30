using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace APIGateway;

/// <summary>
/// The startup class.
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
    /// Gets configuration property.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Configure services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        this.ConfigureAuthentication(services);

        services.AddControllers();

        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddHttpClient();

        services.AddOcelot(this.Configuration)
            .AddCacheManager(x =>
            {
                x.WithDictionaryHandle();
            });
    }

    /// <summary>
    /// Configure application.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <param name="env">Web host environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.EnvironmentName == "Docker")
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
                c.SwaggerEndpoint("/swagger/catalog/v1/swagger.json", "Catalog Service API");
                c.SwaggerEndpoint("/swagger/cart/v1/swagger.json", "Cart Service API V1");
                c.SwaggerEndpoint("/swagger/cart/v2/swagger.json", "Cart Service API V2");
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseOcelot().Wait();
    }

    /// <summary>
    /// Configure Authentication.
    /// </summary>
    /// <param name="services">Service collection.</param>
    protected virtual void ConfigureAuthentication(IServiceCollection services)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        var isDocker = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = this.Configuration["Authentication:Authority"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = !isDevelopment && !isDocker,
                    ValidateLifetime = true,
                    RoleClaimType = ClaimTypes.Role,
                };
                options.RequireHttpsMetadata = false;

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var roleClaims = context.Principal?.FindAll(ClaimTypes.Role).ToList();
                        if (roleClaims != null)
                        {
                            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
                            foreach (var roleClaim in roleClaims)
                            {
                                claimsIdentity?.AddClaim(new Claim("role", roleClaim.Value));
                            }
                        }

                        return Task.CompletedTask;
                    },
                };
            });
    }
}