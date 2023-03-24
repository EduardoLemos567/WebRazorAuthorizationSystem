using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public abstract class AAccount : IdentityUser<int>
{
    [Required, ProtectedPersonalData]
    public string? RealName { get; set; }
}
