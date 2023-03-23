using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Login;

public class StaffUserStore : UserStore<StaffAccount>
{
    public StaffUserStore(DataContext db, ILookupNormalizer normalizer) : base(db, normalizer) { }
    protected override DbSet<StaffAccount> Users => db.StaffAccounts;
}
