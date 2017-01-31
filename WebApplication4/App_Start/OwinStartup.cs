using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(WebApplication4.App_Start.OwinStartup))]

namespace WebApplication4.App_Start
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/LogIn")
            });

            string conSt= ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;


            GlobalConfiguration.Configuration
                .UseSqlServerStorage(conSt);

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        
    }
    }
}