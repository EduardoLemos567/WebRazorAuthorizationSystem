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
        if (Util.SelectionIsInvalid(SelectedRoles, AllRoles.Count))
        {
            ModelState.AddModelError("SelectedRoles", "Invalid selection(s)");
        }
        if (!ModelState.IsValid)
        {
            Identity = Models.SummaryIdentity.FromIdentity(user);
            return Page();
        }
        var newRoles = Util.SelectionIntToString(AllRoles, SelectedRoles);
        var oldRoles = await users.GetRolesAsync(user);
        // Remove all previous roles which are not marked on the newRoles. But also dont
        // remove any of the DefaultRoles, they should be immutable.
        var toRemove = oldRoles.Except(newRoles).Except(DefaultRoles.GetEnumerable());
        if (toRemove.Any())
        {
            var removeFromRolesResult = await users.RemoveFromRolesAsync(user, toRemove);
            if (!removeFromRolesResult.Succeeded)
            {
                return Content($"Could not save roles, try again.\nReasons:{string.Join(", ", from e in removeFromRolesResult.Errors select e.Description)}");
            }
        }
        var toAdd = newRoles.Except(oldRoles);
        if (toAdd.Any())
        {
            var addToRolesResult = await users.AddToRolesAsync(user, toAdd);
            if (!addToRolesResult.Succeeded)
            {
                return Content($"Could not save roles, try again.\nReasons:{string.Join(", ", from e in addToRolesResult.Errors select e.Description)}");
            }
        }
        return RedirectToPage("./Index");
    }
}
