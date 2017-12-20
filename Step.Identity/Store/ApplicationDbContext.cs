using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Step.Identity.Manager;
using Step.Identity.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Step.Identity.Store
{
    public class DbInitialize : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        private async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var store = new CustomRoleStore(context);
                var manager = new ApplicationRoleManager(store);
                var role = new AppRole { Name = "AppAdmin" };

                await manager.CreateAsync(role);
                var roleGreo = new AppRole { Name = "AppGreo" };
                await manager.CreateAsync(roleGreo);
            }
            if (!context.Users.Any(u => u.UserName == "dimka@gmail.com"))
            {
                var store = new CustomUserStore(context);
                var manager = new ApplicationUserManager(store);
                var user = new AppUser
                {
                    FirstName="Dmitry", LastName="Chumak",
                    UserName = "dimka@gmail.com",
                    SubdivisionId = 0,
                    Email = "dimka@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0123456789",
                    PhoneNumberConfirmed = true
                };
               

                await manager.CreateAsync(user, "Qwerty123_");

            

                //Add User To Role
                await manager.AddToRoleAsync(user.Id, "AppAdmin");
                ////Add User Claims
                await manager.AddClaimAsync(user.Id, new Claim(ClaimTypes.GivenName, "A Person"));
                await manager.AddClaimAsync(user.Id, new Claim(ClaimTypes.Gender, "Man"));
                await manager.AddClaimAsync(user.Id, new Claim(ClaimTypes.DateOfBirth, "01.01.2001"));

               
            }
            if (!context.Users.Any(u => u.UserName == "greo.401.eng@gmail.com"))
            {
                var store = new CustomUserStore(context);
                var manager = new ApplicationUserManager(store);
                var userGreo1 = new AppUser
                {
                    FirstName = "greo401",
                    LastName = "greo401",
                    UserName = "greo.401.eng@gmail.com",
                    SubdivisionId = 1,
                    Email = "greo.401.eng@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0123456789",
                    PhoneNumberConfirmed = true
                };


                await manager.CreateAsync(userGreo1, "Greo401_");
                //Add User To Role
                await manager.AddToRoleAsync(userGreo1.Id, "AppGreo");
            }
            if (!context.Users.Any(u => u.UserName == "greo.402.eng@gmail.com"))
            {
                var store = new CustomUserStore(context);
                var manager = new ApplicationUserManager(store);
                var userGreo2 = new AppUser
                {
                    FirstName = "greo402",
                    LastName = "greo402",
                    UserName = "greo.402.eng@gmail.com",
                    SubdivisionId = 2,
                    Email = "greo.402.eng@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0123456789",
                    PhoneNumberConfirmed = true
                };


                await manager.CreateAsync(userGreo2, "Greo402_");
                //Add User To Role
                await manager.AddToRoleAsync(userGreo2.Id, "AppGreo");
            }


        }


        protected override void Seed(ApplicationDbContext context)
        {

            Task.Run(async () => { await SeedAsync(context); }).Wait();

            base.Seed(context);
        }
    }
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
       
        public ApplicationDbContext()
            : base()
        {
            if (!Database.Exists())
            {
                Database.SetInitializer<ApplicationDbContext>(new DbInitialize());
            }

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
