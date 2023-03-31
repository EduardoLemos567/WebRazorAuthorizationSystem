using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Data;
using Project.Models;

namespace Project.Startup;

public static class Startup
{
    #region BEFORE_BUILD
    public static void AddServices(IServiceCollection services, ConfigurationManager configurator)
    {
        // Add services to the container.
        services.AddRazorPages();
        AddDatabaseService(services, configurator);
        AddLoginService(services);
        // Add initial sorted permissions list as option
        services.AddSingleton<CachedDefaultData>();
    }
    private static void AddDatabaseService(IServiceCollection services, ConfigurationManager configurator)
    {
        services.AddDbContext<DataDbContext>(o =>
            o.UseSqlite(
                configurator.GetConnectionString("DataContextPath") ??
                throw new InvalidOperationException("Connection string 'DataContextPath' not found.")));
    }
    private static void AddLoginService(IServiceCollection services)
    {
        services.AddIdentity<Identity, Role>(AddIdentityOptions)
            .AddUserStore<UserStore<Identity, Role, DataDbContext, int>>()
            .AddRoleStore<RoleStore<Role, DataDbContext, int>>();
        services.ConfigureApplicationCookie(AddCookieAuthenticationOptions);
    }
    #region OPTIONS
    private static void AddIdentityOptions(IdentityOptions options)
    {
        options.SignIn.RequireConfirmedAccount = false; // TODO: for testing, we dont need to confirm account.
        // Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        // User settings.
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    }
    private static void AddCookieAuthenticationOptions(CookieAuthenticationOptions options)
    {
        // Cookie settings
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.LogoutPath = "/Account/Logout";
        options.SlidingExpiration = true;
    }
    #endregion OPTIONS
    #endregion BEFORE_BUILD
    #region AFTER_BUILD
    public static void AddInitialData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roles = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("AddInitialData");

            {   // Create roles
                // If any of default roles doesnt exist in Roles, we recreate missing ones.
                var defaultRoles = Enum.GetNames<DefaultRoles>();
                var createdRoles = from r in roles.Roles select r.Name;
                foreach (var name in defaultRoles.Except(createdRoles))
                {
                    var createRoleResult = roles.CreateAsync(new(name)).Result;
                    if (!createRoleResult.Succeeded)
                    {
                        logger.LogWarning("Could not create default role {name}.", name);
                    }
                }
            }
            {   // Add permissions to admin role
                // Get admin role
                var adminRole = roles.FindByNameAsync(DefaultRoles.Admin.ToString()).Result;
                if (adminRole is null)
                {
                    logger.LogWarning("Could not find admin role.");
                    return;
                }
                // Add all permissions
                var oldClaims = roles.GetClaimsAsync(adminRole!).Result;
                if ((from c in oldClaims where c.Type == Requirements.PERMISSIONS_CLAIM_TYPE select c).Any())
                {
                    logger.LogInformation("Admin role already contains permissions.");
                }
                else
                {
                    var addClaimResult = roles.AddClaimAsync(adminRole!,
                            new(Requirements.PERMISSIONS_CLAIM_TYPE,
                                Requirements.AllRequiredPermissionsString())).Result;
                    if (!addClaimResult.Succeeded)
                    {
                        logger.LogWarning("Could not add all permissions to admin role.");
                    }
                }
            }
            {
                // Create admin identity
                var users = scope.ServiceProvider.GetRequiredService<UserManager<Identity>>();
                if (users.FindByNameAsync("Admin").Result is not null)
                {
                    logger.LogInformation("Admin was already created.");
                    return;
                }
                var identity = new Identity
                {
                    UserName = "Admin",
                    Email = "admin@admin.com",
                };
                var userCreationResult = users.CreateAsync(identity, "Admin1&").Result;
                if (!userCreationResult.Succeeded)
                {
                    logger.LogWarning("Could not create admin identity.");
                    return;
                }
                // Add admin identity to Admin role
                var addToAdminRoleResult = users.AddToRoleAsync(identity, DefaultRoles.Admin.ToString()).Result;
                if (!addToAdminRoleResult.Succeeded)
                {
                    logger.LogWarning("Could not add admin identity to admin role.");
                    return;
                }
                var addToStaffRoleResult = users.AddToRoleAsync(identity, DefaultRoles.Staff.ToString()).Result;
                if (!addToStaffRoleResult.Succeeded)
                {
                    logger.LogWarning("Could not add admin identity to staff role.");
                    return;
                }
                logger.LogInformation("Admin user created.");
            }
        }
    }
    public static void SetupSeedData(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            using (var seeder = new DataSeeder(scope))
            {
                seeder.SeedAllModels();
            }
        }
    }
    public static void AddMiddlewares(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for
            // production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapRazorPages();
    }
    #endregion AFTER_BUILD
}
