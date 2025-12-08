using System.Security.Claims;
using Duende.IdentityServer.Services;
using IdentityServer.Configuration;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
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
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">Throws when connection string was not found.</exception>
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryClients(Config.GetClients())
            .AddAspNetIdentity<AppUser>()
            .AddDeveloperSigningCredential();

            builder.Services.AddTransient<IProfileService, ProfileService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await SeedRolesAndPermissions(app);

            await app.RunAsync();
        }

        private static async Task SeedRolesAndPermissions(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("Manager"))
            {
                var managerRole = new IdentityRole("Manager");
                await roleManager.CreateAsync(managerRole);

                await roleManager.AddClaimAsync(managerRole, new Claim("permission", Permissions.Read));
                await roleManager.AddClaimAsync(managerRole, new Claim("permission", Permissions.Create));
                await roleManager.AddClaimAsync(managerRole, new Claim("permission", Permissions.Update));
                await roleManager.AddClaimAsync(managerRole, new Claim("permission", Permissions.Delete));
            }

            if (!await roleManager.RoleExistsAsync("StoreCustomer"))
            {
                var storeCustomerRole = new IdentityRole("StoreCustomer");
                await roleManager.CreateAsync(storeCustomerRole);

                await roleManager.AddClaimAsync(storeCustomerRole, new Claim("permission", Permissions.Read));
            }
        }
    }
}
