using System.Reflection;
using AutoMapper;
using Catalog.Core.MappingProfiles;
using Catalog.Core.Messaging;
using Catalog.Core.Repositories;
using Catalog.Core.Services;
using Catalog.Core.Services.Interfaces;
using Catalog.Persistence.DBContext;
using Catalog.Persistence.MappingProfiles;
using Catalog.Persistence.Repositories;
using Catalog.WebAPI.Controllers;
using Catalog.WebAPI.GraphQL.DataLoaders;
using Catalog.WebAPI.GraphQL.Mutations;
using Catalog.WebAPI.GraphQL.Queries;
using Catalog.WebAPI.GraphQL.Types;
using Catalog.WebAPI.Mapping;
using Catalog.WebAPI.Messaging;
using Catalog.WebAPI.Middlewares;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.WebAPI
{
    /// <summary>
    /// Startup class.
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
            var config = new MapperConfiguration(
                c =>
                {
                    c.AddProfile<CategoryProfile>();
                    c.AddProfile<ProductProfile>();
                    c.AddProfile<ProductDtosProfile>();
                    c.AddProfile<CategoryDtosProfile>();
                    c.AddProfile<ProductMessageProfile>();
                }, new NullLoggerFactory());

            services.AddSingleton<IMapper>(s => config.CreateMapper());

            this.ConfigureAuthentication(services);
            ConfigureAuthorization(services);

            services.AddControllers()
                .AddApplicationPart(typeof(CategoriesController).Assembly);

            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.Configure<KafkaConfig>(this.Configuration.GetSection("Kafka"));
            services.AddSingleton<IProductUpdatePublisher, ProductUpdatePublisher>();

            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<CatalogQuery>()
                .AddMutationType<CatalogMutations>()
                .AddType<CategoryType>()
                .AddType<ProductType>()
                .AddDataLoader<CategoryDataLoader>()
                .AddDataLoader<ProductDataLoader>()
                .AddDataLoader<ProductsByCategoryDataLoader>();

            this.ConfigureDbContext(services);
        }

        /// <summary>
        /// Configure application.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Web host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenAPI v1");
                });
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL()
                    .WithOptions(new GraphQLServerOptions
                    {
                        Tool = { Enable = env.IsDevelopment() },
                    });
            });
        }

        /// <summary>
        /// Configure DB Context.
        /// </summary>
        /// <param name="services">Service collection.</param>
        protected virtual void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<CatalogDBContext>(options =>
                options.UseNpgsql(this.Configuration.GetConnectionString("DefaultConnection")));
        }

        /// <summary>
        /// Configure Authentication.
        /// </summary>
        /// <param name="services">Service collection.</param>
        protected virtual void ConfigureAuthentication(IServiceCollection services)
        {
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = this.Configuration["Authentication:Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = !isDevelopment,
                        ValidateLifetime = true,
                    };
                    options.RequireHttpsMetadata = false;
                });
        }

        /// <summary>
        /// Configure Authorization policies.
        /// </summary>
        /// <param name="services">Service collection.</param>
        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("Read", policy =>
                    policy.RequireClaim("permission", "Read"))
                .AddPolicy("Create", policy =>
                    policy.RequireClaim("permission", "Create")
                          .RequireRole("Manager"))
                .AddPolicy("Update", policy =>
                    policy.RequireClaim("permission", "Update")
                          .RequireRole("Manager"))
                .AddPolicy("Delete", policy =>
                    policy.RequireClaim("permission", "Delete")
                          .RequireRole("Manager"));
        }
    }
}
