using System.Reflection;
using Catalog.Core.Repositories;
using Catalog.Core.Services.Interfaces;
using Catalog.Core.Services;
using Catalog.Persistence.MappingProfiles;
using Catalog.WebAPI.Middlewares;
using Catalog.Persistence.Repositories;
using AutoMapper;
using Catalog.Persistence.DBContext;
using Catalog.WebAPI.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Catalog.Core.Messaging;
using Catalog.WebAPI.Controllers;
using Catalog.WebAPI.Messaging;

namespace Catalog.WebAPI
{
    /// <summary>
    /// Startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets configuration property.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

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
                }, new NullLoggerFactory());

            services.AddSingleton<IMapper>(s => config.CreateMapper());

            services.AddControllers()
                .AddApplicationPart(typeof(CategoriesController).Assembly);

            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.Configure<KafkaConfig>(this.Configuration.GetSection("Kafka"));
            services.AddSingleton<IProductUpdatePublisher, ProductUpdatePublisher>();

            this.ConfigureDbContext(services);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
