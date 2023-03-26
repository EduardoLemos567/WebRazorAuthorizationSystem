using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data;

public class DataDbContext : DbContext
{
    public DataDbContext(DbContextOptions<DataDbContext> options)
        : base(options)
    { }
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<MovieCategory> MovieCategories { get; set; } = default!;
    public DbSet<UserAccount> UserAccounts { get; set; } = default!;
    public DbSet<StaffAccount> StaffAccounts { get; set; } = default!;
    public DbSet<PermissionsPackage> PermissionsPackages { get; set; } = default!;
    public DbSet<StaffRole> StaffRoles { get; set; } = default!;
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) =>
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
}

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    { }
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<MovieCategory> MovieCategories { get; set; } = default!;
    public DbSet<UserAccount> UserAccounts { get; set; } = default!;
    public DbSet<StaffAccount> StaffAccounts { get; set; } = default!;
    public DbSet<PermissionsPackage> PermissionsPackages { get; set; } = default!;
    public DbSet<StaffRole> StaffRoles { get; set; } = default!;
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) =>
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
}