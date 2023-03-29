using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data;

public class DataSeeder : IDisposable
{
    private readonly DataDbContext db;
    private readonly Random rng;
    public DataSeeder(IServiceScope scope)
    {
        db = new DataDbContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<DataDbContext>>());
        this.rng = new Random(1234);
    }
    public void SeedAllModels()
    {
        this.SeedMovieCategories();
        this.SeedMovies();
    }
    public void SeedMovieCategories()
    {
        if (this.db.MovieCategories.Any()) { return; }
        foreach (var i in Enumerable.Range(0, 10))
        {
            this.db.Add(new MovieCategory()
            {
                Name = $"MovieCategory {i + 1}"
            });
        }
        this.db.SaveChanges();
    }
    public void SeedMovies()
    {
        if (this.db.Movies.Any()) { return; }
        var categories = this.db.MovieCategories.ToList();
        foreach (var i in Enumerable.Range(0, 20))
        {
            this.db.Add(new Movie()
            {
                Name = $"Movie {i + 1}",
                ReleaseDate = new(this.rng.Next(1900, 2024), this.rng.Next(1, 13), this.rng.Next(1, 31)),
                Category = categories[this.rng.Next(categories.Count)],
            });
        }
        this.db.SaveChanges();
    }
    public void Dispose()
    {
        db.Dispose();
        GC.SuppressFinalize(this);
    }
}
