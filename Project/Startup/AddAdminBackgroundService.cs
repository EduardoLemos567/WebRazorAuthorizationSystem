using Microsoft.AspNetCore.Identity;
using Project.Models;
using Project.Permissions;

namespace Project.Startup;

public class AddAdminBackgroundService : IHostedService
{
    private readonly ILogger<AddAdminBackgroundService> logger;
    private readonly IHostApplicationLifetime lifetime;
    private readonly IServiceProvider services;
    private readonly PermissionService permissions;
    public AddAdminBackgroundService(ILogger<AddAdminBackgroundService> logger,
                                     IHostApplicationLifetime lifetime,
                                     IServiceProvider services,
                                     PermissionService permissions)
    {
        this.logger = logger;
        this.lifetime = lifetime;
        this.services = services;
        this.permissions = permissions;
    }
    public Task StartAsync(CancellationToken _)
    {
        lifetime.ApplicationStarted.Register(ExecuteInSync);
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    private void ExecuteInSync()
    {
        //TODO: execute async
    }
    private async Task ExecuteAsync(CancellationToken cancelationToken)
    {
        if (cancelationToken.IsCancellationRequested) { return; }
        using (var scope = services.CreateScope())
        {
            var users = scope.ServiceProvider.GetRequiredService<UserManager<StaffAccount>>();
            var roles = scope.ServiceProvider.GetRequiredService<IRoleStore<StaffRole>>();
            var roleCreationResult = await roles.CreateAsync(new("Admin"), cancelationToken);
            if (!roleCreationResult.Succeeded)
            {
                logger.LogWarning($"Couldn't create admin role, error: {string.Join(',', from e in roleCreationResult.Errors select e.Description)}");
            }
            var admin = new StaffAccount()
            {
                UserName = "Admin",
                RealName = "Admin",
                Email = "admin@admin.com",
                Permissions = new((from p in permissions.AllPermissions select p.data).ToArray())
            };
            admin.SortPermissions();
            var userCreationResult = await users.CreateAsync(admin, "Admin1&");
            if (!userCreationResult.Succeeded)
            {
                logger.LogWarning($"Couldn't create admin user, error: {string.Join(',', from e in userCreationResult.Errors select e.Description)}");
            }
            var addToRoleResult = await users.AddToRoleAsync(admin, "Admin");
            if (!addToRoleResult.Succeeded)
            {
                logger.LogWarning($"Couldn't add admin user to admin role, error: {string.Join(',', from e in addToRoleResult.Errors select e.Description)}");
            }
            logger.LogInformation("Admin user created.");
        }
    }
}
