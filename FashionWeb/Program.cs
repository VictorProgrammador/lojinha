using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FashionWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //QUANDO QUISER ADICIONAR MAIS REGRAS E PERMISSÕES
        //public async static Task Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();


        //    var host = CreateHostBuilder(args).Build();
        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var services = scope.ServiceProvider;

        //        try
        //        {
        //            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        //            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        //            await Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
        //            await roleManager.SeedClaimsForNegociante();
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //    }

        //    host.Run();
        //}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
