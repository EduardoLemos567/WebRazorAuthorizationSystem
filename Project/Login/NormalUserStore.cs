using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Login;

public class NormalUserStore : UserStore<UserAccount>
{
    public NormalUserStore(DataContext db, ILookupNormalizer normalizer) : base(db, normalizer) { }
    protected override DbSet<UserAccount> Users => db.UserAccounts;
}
