using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Login;

public abstract class UserStore<TUser> : IUserStore<TUser> where TUser : IdentityUser<int>
{
    protected readonly DataContext db;
    private readonly ILookupNormalizer normalizer;
    private bool disposed;
    protected UserStore(DataContext db, ILookupNormalizer normalizer)
    {
        this.db = db;
        this.normalizer = normalizer;
    }
    protected abstract DbSet<TUser> Users { get; }
    public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }
    public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        Users.Remove(user);
        await db.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }
    public Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var id = ConvertIdFromString(userId);
        return Users.FindAsync(new object?[] { id }, cancellationToken).AsTask();
    }
    public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
    }
    public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        string? result = default;
        if (user.NormalizedUserName != null)
        {
            result = user.NormalizedUserName;
        }
        if (user.UserName != null)
        {
            result = normalizer.NormalizeName(user.UserName);
        }
        return Task.FromResult(result);
    }
    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return Task.FromResult(ConvertIdToString(user.Id)!);
    }
    public Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return Task.FromResult(user.UserName);
    }
    public Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }
    public Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        user.UserName = userName;
        return Task.CompletedTask;
    }
    public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
    public void Dispose() => disposed = true;
    private void ThrowIfDisposed()
    {
        if (disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
    private static int? ConvertIdFromString(string? id)
    {
        if (id == null)
        {
            return default(int);
        }
        if (int.TryParse(id, out var number))
        {
            return number;
        }
        return default;
    }
    private static string? ConvertIdToString(int id)
    {
        if (id == default)
        {
            return null;
        }
        return id.ToString();
    }
}
