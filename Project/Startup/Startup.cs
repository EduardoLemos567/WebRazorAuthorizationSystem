using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Login;
using Project.Models;
using Project.Permissions;

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
        services.AddSingleton<PermissionService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddHostedService<SetupBackgroundService>();
    }
    private static void AddDatabaseService(IServiceCollection services, ConfigurationManager configurator)
    {
        services.AddDbContext<DataContext>(o =>
            o.UseSqlite(
                configurator.GetConnectionString("DataContextPath") ??
                throw new InvalidOperationException("Connection string 'DataContextPath' not found.")));
    }
    private static void AddLoginService(IServiceCollection services)
    {
        IdentityBuilder BuildIdentity<TAccount>() where TAccount : AAccount
        {
            services.AddScoped<AuthenticationService<TAccount>>();
            services.AddScoped<IUserStore<TAccount>, UserStore<TAccount>>();
            services.AddScoped<SignInManager<TAccount>>();
            return services.AddIdentityCore<TAccount>(AddIdentityOptions);
        }
        BuildIdentity<UserAccount>();
        BuildIdentity<StaffAccount>()
            .AddRoles<StaffRole>();
        services.AddScoped<IRoleStore<StaffRole>, RoleStore>();
        services.ConfigureApplicationCookie(AddCookieAuthenticationOptions);
    }
    #region OPTIONS
    private static void AddIdentityOptions(IdentityOptions options)
    {
        options.SignIn.RequireConfirmedAccount = true;
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
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
    }
    #endregion OPTIONS
    #endregion BEFORE_BUILD
    #region AFTER_BUILD
    public static void SetupSeedData(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            using (var db = new DataContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
            {
                var seeder = new DataSeeder(db);
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
        app.UseMiddleware<PermissionsMiddleWare>();
        app.MapRazorPages();
    }
    #endregion AFTER_BUILD
}
