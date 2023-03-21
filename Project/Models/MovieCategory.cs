using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class MovieCategory
{
    [Required, Key]
    public int Id { get; set; }
    [Required, StringLength(500, MinimumLength = 3)]
    public string? Name { get; set; }
}
