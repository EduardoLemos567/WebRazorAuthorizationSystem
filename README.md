# Sample Project

This is a sample project using:
 - C# 11 	
 -  .NET Core 7.0 	
 - ASP.NET Core 7.0.4
 - EF Core 7.0.4
 - Razor Pages Model

## Description
I've used this project to learn details about the ASP.NET.
Some decisions are made towards learning instead of just following 
every tutorial/sample on the internet (which i did too).

My initial intention was to have two accounts models: StaffAccount and UserAccount.
</br>
StaffAccount, having the roles and permissions to navigate on the site's "admin panel".
</br>
While the UserAccount would be to hold user/clients information, related to the business.

I would have to reimplement a lot of the Identity features like: UserManager, UserStore, RoleStore, etc.

I've decided to embrace the identity model. If needed to separate these two previous models, i would use different profiles.
I would create a Profile model: StaffProfile and UserProfile, allowing both to point to a single identity.
Allowing a Staff to also be a User/Client and have its particular data separated, while maintaing the single
identity for login and security.

I've used the roles model to hold differences between a User, Staff or Admin and allowing the admin to include
any other role needed through the admin panel.

A identity can be a User (client), a Staff (having other positions like manager, finances, marketing being added later)
and the Admin (single staff user with all power for configurations).

Since i learned that claims can be stored per identity as well per role. 
When a identity user logs in, all the claims from the user are concatenated with the roles claims.

This is perfect for a permission model where a identity and a role can have different sets of permissions.
For example: you can have your staff user without any custom permission, and add/remove him to a role where
a different set of permissions are granted.
</br> 
All these permissions stored/modified per role.

## Permission Model

My permission model is stored as a claim, where in the string, each single char is a permission.
</br>
Its not human readable, but we are able to store more permissions this way.

I've created the attribute "RequirePermission", where the dev can inform the place and an action.
A place is a finality and the action is one of the CRUD, including others like listing.

For example: 

If a staff user need to access the page: /Admin/Identity/Edit, 
which is marked with the attribute: 
`[RequirePermission(Places.Identity, Actions.Update)]`

Into his claims he need to have the `PERMISSIONS_CLAIM_TYPE = "Permissions"`, in which one the the characters
of the value string, must be equals to the permission character.

A permission character is formed by:
<table>
    <tbody>
        <tr>
            <td colspan=2 style="text-align: center">
                16 bits (char)
            </td>
        </tr>
        <tr>
            <td>Place</td>
            <td>Action</td>
        </tr>
        <tr>
            <td>13 bits</td>
            <td>3 bits</td>
        </tr>
    </tbody>
</table>



This way we are allowed to combine 8191 different places with 8 different actions.

Having a permission of the size of a single character, we are able to store each permission in a compact way, 
without any spaces or separator character.

We can also extract a readable permission list to build a view to grant/revoke by calling `Requirements.AllRequiredPermissions()`.