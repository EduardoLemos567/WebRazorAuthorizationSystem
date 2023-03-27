using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class LoginInfo
{
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public bool RememberMe { get; set; }
}
