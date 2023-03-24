using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Pages.Admin.StaffAccount
{
    public class DeleteModel : PageModel
    {
        private readonly DataContext db;
        public DeleteModel(DataContext context)
        {
            db = context;
        }
        [BindProperty]
        public Models.StaffAccount StaffAccount { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || db.StaffAccounts == null)
            {
                return NotFound();
            }

            var account = await db.StaffAccounts.FirstOrDefaultAsync(m => m.Id == id);

            if (account == null)
            {
                return NotFound();
            }
            else
            {
                StaffAccount = account;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || db.StaffAccounts == null)
            {
                return NotFound();
            }
            var account = await db.StaffAccounts.FindAsync(id);

            if (account != null)
            {
                StaffAccount = account;
                db.StaffAccounts.Remove(StaffAccount);
                await db.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
