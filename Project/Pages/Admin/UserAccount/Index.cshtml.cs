using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Pages.Admin.UserAccount
{
    public class IndexModel : PageModel
    {
        private readonly DataContext db;
        public IndexModel(DataContext context)
        {
            db = context;
        }
        public IList<Models.UserAccount> UserAccounts { get; set; } = default!;
        public async Task OnGetAsync()
        {
            if (db.UserAccounts != null)
            {
                UserAccounts = await db.UserAccounts.ToListAsync();
            }
        }
    }
}
