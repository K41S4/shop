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
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Options;

namespace Catalog.WebAPI
{
    /// <summary>
    /// Program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="args">Args.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = new MapperConfiguration(
                c =>
                {
                c.AddProfile<CategoryProfile>();
                c.AddProfile<ProductProfile>();
                c.AddProfile<ProductDtosProfile>();
                c.AddProfile<CategoryDtosProfile>();
            }, new NullLoggerFactory());

            builder.Services.AddSingleton<IMapper>(s => config.CreateMapper());

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddDbContext<CatalogDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenAPI v1");
                });
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
