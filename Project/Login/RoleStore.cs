using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Login;

public class RoleStore
    : RoleStore<StaffRole, DataContext, int>
{
    public RoleStore(DataContext context,
                     IdentityErrorDescriber? describer = null)
        : base(context, describer) { }
}