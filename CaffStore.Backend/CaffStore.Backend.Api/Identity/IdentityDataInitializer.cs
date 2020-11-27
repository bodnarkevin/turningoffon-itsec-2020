using CaffStore.Backend.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Roles;

namespace CaffStore.Backend.Api.Identity
{
	public static class IdentityDataInitializer
	{
		public static async Task SeedDataAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			await SeedRolesAsync(roleManager);
			await SeedUsersAsync(userManager);
		}

		public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
		{
			var exists = await roleManager.RoleExistsAsync(CaffStoreRoles.Admin);

			if (exists)
				return;

			Role role = new Role
			{
				Name = CaffStoreRoles.Admin
			};

			await roleManager.CreateAsync(role);
		}

		public static async Task SeedUsersAsync(UserManager<User> userManager)
		{
			var adminUserName = "admin@admin";
			var adminPassword = "Admin1234";

			var exists = await userManager.FindByNameAsync(adminUserName) != null;

			if (exists)
				return;

			User user = new User
			{
				UserName = adminUserName,
				Email = adminUserName,
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
