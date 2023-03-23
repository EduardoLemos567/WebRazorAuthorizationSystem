using Project.Login;
using Project.Models;
using Project.Permissions;

internal class PostRunSetupService : IHostedService
{
    private readonly ILogger<PostRunSetupService> logger;
    private readonly IHostApplicationLifetime lifetime;
    private readonly IServiceProvider provider;
    private readonly PermissionService permissor;
    public PostRunSetupService(ILogger<PostRunSetupService> logger,
                        IHostApplicationLifetime lifetime,
                        IServiceProvider provider,
                        PermissionService permissor)
    {
        this.logger = logger;
        this.lifetime = lifetime;
        this.provider = provider;
        this.permissor = permissor;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        lifetime.ApplicationStarted.Register(OnStarted);
        return Task.CompletedTask;
    }
    private async void OnStarted()
    {
        var admin = new StaffAccount()
        {
            UserName = "Admin",
            Email = "admin@admin.com",
            Permissions = new((from p in permissor.AllPermissions select p.data).ToArray())
        };
        admin.SortPermissions();
        using (var scope = provider.CreateScope())
        {
            var staffAuth = scope.ServiceProvider.GetService<AuthenticationService<StaffAccount>>();
            var creationResult = await staffAuth!.TryCreateConfirmedAccount(admin, "admin");
            logger.LogDebug(
                creationResult.Succeeded ? "Admin user created."
                : $"Couldn't create admin user, error: {string.Join(',', from e in creationResult.Errors select e.Description)}");
        }
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
