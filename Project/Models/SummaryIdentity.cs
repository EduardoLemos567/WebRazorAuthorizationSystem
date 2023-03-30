using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class SummaryIdentity
{
    public int Id { get; set; }
    public string? Alias { get; set; }
    [Required, Display(Name = "Full name")]
    public string? UserName { get; set; }
    [Required, DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [DataType(DataType.PhoneNumber), Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }
    public void Update(Identity i)
    {
        i.Alias = Alias;
        i.UserName = UserName;
        i.Email = Email;
        i.PhoneNumber = PhoneNumber;
    }
    public static SummaryIdentity FromIdentity(Identity i)
    {
        return new SummaryIdentity
        {
            Id = i.Id,
            Alias = i.Alias,
            UserName = i.UserName,
            Email = i.Email,
            PhoneNumber = i.PhoneNumber,
        };
    }
}
