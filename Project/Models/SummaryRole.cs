using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class SummaryRole
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; } = default!;
    public void Update(Role r)
    {
        r.Name = Name;
    }
    public static SummaryRole FromRole(Role r)
    {
        return new SummaryRole
        {
            Id = r.Id,
            Name = r.Name,
        };
    }
}
