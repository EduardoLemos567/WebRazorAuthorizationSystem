﻿using Microsoft.AspNetCore.Identity;
using Project.Data;

namespace Project.Login;

public class UserStore<TUser> : IUserStore<TUser> where TUser : class
{
    private readonly DataContext db;
    public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
    public Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
}
