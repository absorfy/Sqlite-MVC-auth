using Microsoft.AspNetCore.Identity;

namespace MyMVCAppAuth.Data;

public static class AdminAndRolesInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = ["Admin", "User"];
        foreach (var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
        
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = "admin@localhost.com",
                Email = "admin@localhost.com",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            if (!adminUser.EmailConfirmed)
            {
                adminUser.EmailConfirmed = true;
                await userManager.UpdateAsync(adminUser);
            }
            if(!await userManager.IsInRoleAsync(adminUser, "Admin"))
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}