using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class Movie
{
    [Required, Key]
    public int Id { get; set; }
    [Required, StringLength(500, MinimumLength = 3)]
    public string? Name { get; set; }
    [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly ReleaseDate { get; set; }
    [Required]
    public MovieCategory? Category { get; set; }
}
