using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Services;
using Project.Utils;

namespace Project.Pages.Admin.Enrole;

[RequirePermission(Places.Enrole, Actions.Update)]
public class EditModel : CrudPageModel
{
    public EditModel(AdminRules rules, UserManager<Models.Identity> users) : base(rules, users) { }
    public Models.SummaryIdentity Identity { get; set; } = default!;
    public List<string> AllRoles { get; set; } = default!;
    [BindProperty]
    public List<int> SelectedRoles { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var user = await rules.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        if (!await rules.CanModifyUserRolesAsync(user))
        {
            return Content("Cant modify this user as its not a Staff account");
        }
        Identity = Models.SummaryIdentity.FromIdentity(user);
        AllRoles = await (from r in rules.GetAllNonDefaultRoles() select r.Name).ToListAsync();
        SelectedRoles = Util.SelectionStringToInt(AllRoles, rules.GetUserNonDefaultRoles(user)).ToList();
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var user = await rules.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        if (!await rules.CanModifyUserRolesAsync(user))
        {
            return Content("Cant modify this user as its not a Staff account");
        }
        AllRoles = await (from r in rules.GetAllNonDefaultRoles() select r.Name).ToListAsync();
        if (!Util.SelectionIsValid(SelectedRoles, AllRoles.Count))
        {
            ModelState.AddModelError("SelectedRoles", "Invalid selection(s)");
        }
        if (!ModelState.IsValid)
        {
            Identity = Models.SummaryIdentity.FromIdentity(user);
            return Page();
        }
        var addToRolesResult = await users.AddToRolesAsync(user, Util.SelectionIntToString(AllRoles, SelectedRoles));
        if (!addToRolesResult.Succeeded)
        {
            return Content($"Could not save roles, try again.\nReasons:{string.Join(", ", from e in addToRolesResult.Errors select e.Description)}");
        }
        return RedirectToPage("./Index");
    }
}
