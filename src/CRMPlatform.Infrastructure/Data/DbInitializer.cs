using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using CRMPlatform.Domain.Constants;
using CRMPlatform.Domain.Entities;

namespace CRMPlatform.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roleNames = { AppRoles.Admin, AppRoles.Manager, AppRoles.SalesExecutive };

        // 1. Seed Roles
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 2. Seed Default Admin User
        const string adminEmail = "admin@crmplatform.local";
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var createResult = await userManager.CreateAsync(adminUser, "Admin@123456");

            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
            }
            else
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create default Admin: {errors}");
            }
        }
    }
}