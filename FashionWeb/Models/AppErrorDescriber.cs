using Microsoft.AspNetCore.Identity;

namespace FashionWeb.Models
{
    public class AppErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            var error = base.DuplicateUserName(userName);
            error.Description = "Esse Email já está sendo usado. Faça seu login.";
            return error;
        }
    }
}
