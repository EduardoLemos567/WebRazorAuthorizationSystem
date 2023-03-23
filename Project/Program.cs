using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Login;
using Project.Models;
using Project.Permissions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        AddServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        AddSeedData(app);
        AddMiddlewares(app);
        app.Run();
    }
    private static void AddServices(IServiceCollection services, ConfigurationManager configurator)
    {
        // Add services to the container.
        services.AddRazorPages();
        AddDatabaseService(services, configurator);
        AddLoginService(services);
        services.AddSingleton<PermissionService>();
        services.AddHostedService<PostRunSetupService>();
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
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        // UserAccount services
        AddIdentityCore<UserAccount>(services);
        services.AddScoped<UserManager<UserAccount>>();
        services.AddScoped<SignInManager<UserAccount>>();
        services.AddScoped<AuthenticationService<UserAccount>>();
        services.AddScoped<IUserStore<UserAccount>, NormalUserStore>();
        // StaffAccount services
        AddIdentityCore<StaffAccount>(services);
        services.AddScoped<UserManager<StaffAccount>>();
        services.AddScoped<SignInManager<StaffAccount>>();
        services.AddScoped<AuthenticationService<StaffAccount>>();
        services.AddScoped<IUserStore<StaffAccount>, StaffUserStore>();
        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });
    }
    private static void AddIdentityCore<TUser>(IServiceCollection services) where TUser : IdentityUser<int>
    {
        services.AddIdentityCore<TUser>(options =>
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
        });
    }
    private static void AddSeedData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using (var db = new DataContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
            {
                var seeder = new DataSeeder(db);
                seeder.SeedAllModels();
            }
        }
    }
    private static void AddMiddlewares(WebApplication app)
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
        app.UseAuthentication();
        app.UseMiddleware<PermissionsMiddleWare>();
        app.MapRazorPages();
    }
}
