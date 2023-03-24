using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models;
using Project.Permissions;

namespace Project.Startup;

public class SetupBackgroundService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    /* TODO:
    private void SetupAdminAccount()
    {
        using (var scope = services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var users = scope.ServiceProvider.GetRequiredService<UserManager<StaffAccount>>();
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            var permissions = scope.ServiceProvider.GetRequiredService<PermissionService>();
            var admin = new StaffAccount()
            {
                UserName = "Admin",
                RealName = "Admin",
                Email = "admin@admin.com",
                Permissions = new((from p in permissions.AllPermissions select p.data).ToArray())
            };
            admin.SortPermissions();
            var creationResult = await users.CreateAsync(admin, "Admin1&");
            db.StaffRoles.Add(new("Admin"));
            await db.SaveChangesAsync();
            var addToRoleResult = await users.AddToRoleAsync(admin, "Admin");
            if (creationResult.Succeeded)
            {
                logger.LogInformation("Admin user created.");
            }
            else
            {
                logger.LogWarning($"Couldn't create admin user, error: {string.Join(',', from e in creationResult.Errors select e.Description)}");
            }
        }
    }
    */
}
