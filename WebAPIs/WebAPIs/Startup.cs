using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using WebAPIs.Providers;

[assembly: OwinStartup(typeof(WebAPIs.Startup))]

namespace WebAPIs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var a = DatabaseHelper.GetInstance().conn;
        }
    }
}
