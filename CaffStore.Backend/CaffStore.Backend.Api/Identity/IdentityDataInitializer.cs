using CaffStore.Backend.Bll.Options.AdminUser;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Roles;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CaffStore.Backend.Api.Identity
{
	public static class IdentityDataInitializer
	{
		public static async Task SeedDataAsync(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
		{
			await SeedRolesAsync(roleManager);
			await SeedUsersAsync(userManager, configuration);
		}

		public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
		{
			var exists = await roleManager.RoleExistsAsync(CaffStoreRoles.Admin);

			if (exists)
				return;

			var role = new Role
			{
				Name = CaffStoreRoles.Admin
			};

			await roleManager.CreateAsync(role);
		}

		public static async Task SeedUsersAsync(UserManager<User> userManager, IConfiguration configuration)
		{
			var adminUserOptions = new AdminUserOptions();
			configuration.Bind(nameof(AdminUserOptions), adminUserOptions);

			var adminUserEmail = adminUserOptions.DefaultAdminUserEmail;
			var adminPassword = adminUserOptions.DefaultAdminUserPassword;

			var exists = await userManager.FindByNameAsync(adminUserEmail) != null;

			if (exists)
				return;

			var user = new User
			{
				UserName = adminUserEmail,
				Email = adminUserEmail,
				FirstName = "Boss",
				LastName = "Man"
			};

			var result = await userManager.CreateAsync(user, adminPassword);

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, CaffStoreRoles.Admin);
			}
		}
	}
}
