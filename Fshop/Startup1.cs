using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Collections;
using System.Collections.Generic;
[assembly: OwinStartup(typeof(Fshop.Startup1))]
namespace Fshop
{
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security.Cookies;
    using Step.Identity.Manager;
    using Step.Identity.Model;
    using Step.Identity.Store;
    using System.Diagnostics;
    using System.IO;
    using System.Web;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class MyModule1 : OwinMiddleware
    {
        public MyModule1(OwinMiddleware mid) : base(mid) { }
        public override Task Invoke(IOwinContext context)
        {
            RequestCookieCollection cool = context.Request.Cookies;
            if ((cool["Auth"] == null) && (context.Request.Path.ToString().EndsWith("ecret")))
                return context.Response.WriteAsync("<h1>Hey!!</h1>");
            return Next.Invoke(context);

        }
    }
    public class Startup1
    {
        public static Func<UserManager<AppUser, int>> UserManagerFactory { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Authorise/login")
            });
            UserManagerFactory = () =>
            {
                var usermanager = new UserManager<AppUser, int>(
                    new CustomUserStore(new ApplicationDbContext()));
                // добавляем при необходимости валидацию!!!
                usermanager.UserValidator = new UserValidator<AppUser, int>(usermanager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };

                return usermanager;
            };
        }

    }


}
