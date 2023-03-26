using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Login;

public class UserStore<TUser>
    : UserStore<TUser, StaffRole, DataDbContext, int>
    where TUser : AAccount
{
    public UserStore(DataDbContext context,
                     IdentityErrorDescriber? describer = null)
        : base(context, describer) { }
}
