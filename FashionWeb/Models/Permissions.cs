using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FashionWeb.Models
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>() { $"Permissions.{module}.Edit" };
        }

        public static class Profile
        {
            public const string Edit = "Permissions.Profile.Edit";
        }
        public async static Task SeedClaimsForNegociante(this RoleManager<IdentityRole> roleManager)
        {
            var negocianteRole = await roleManager.FindByNameAsync("NEGOCIANTE");
            await roleManager.AddPermissionClaim(negocianteRole, "Profile");
        }
        private static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
