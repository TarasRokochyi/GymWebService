using System.Net;
using Microsoft.AspNetCore.Identity;
using DAL.Constants;

namespace DAL.Models;

public class ContextSeeder
{
    public static async Task SeedEssentialsAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole<int>(AuthorizationConst.Roles.Administrator.ToString()));
        await roleManager.CreateAsync(new IdentityRole<int>(AuthorizationConst.Roles.Moderator.ToString()));
        await roleManager.CreateAsync(new IdentityRole<int>(AuthorizationConst.Roles.User.ToString()));

        //Seed Default User
        var defaultUser = new User { FirstName = "Taras", LastName = "Rokochyi", UserName = AuthorizationConst.default_username, Email = AuthorizationConst.default_email, EmailConfirmed = true, PhoneNumberConfirmed = true };

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            await userManager.CreateAsync(defaultUser, AuthorizationConst.default_password);
            await userManager.AddToRoleAsync(defaultUser, AuthorizationConst.default_role.ToString());
        }
    } 
}