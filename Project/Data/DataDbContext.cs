using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data;

public class DataDbContext : IdentityDbContext<Identity, Role, int>
{
    public DataDbContext(DbContextOptions<DataDbContext> options)
        : base(options)
    { }
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<MovieCategory> MovieCategories { get; set; } = default!;
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) =>
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
}
