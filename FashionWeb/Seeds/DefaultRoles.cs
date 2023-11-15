using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using static FashionWeb.Domain.Entities.UserInfo;

namespace FashionWeb.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.USUARIO.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NEGOCIANTE.ToString()));
        }

        public static bool IsInAllRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.All(r => principal.IsInRole(r));
        }

        public static bool IsInAnyRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.Any(r => principal.IsInRole(r));
        }
    }
}
