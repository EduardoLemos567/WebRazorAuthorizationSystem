using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class AccountCreationInfo
{
    [Required, DataType(DataType.EmailAddress)]
    public string? Email { get; set; } = default!;
    [Required, Display(Name = "Full Name")]
    public string? UserName { get; set; } = default!;
    public string? Alias { get; set; } = default!;
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; } = default!;
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }
}
