using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using MOUNB.BLL.Infrastructure;
using MOUNB.WEB.Util;

namespace MOUNB.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Внедрение зависимостей
            NinjectModule plModule = new PLModule();
            NinjectModule serviceModule = new ServiceModule("DefaultConnection");

            var kernel = new StandardKernel(plModule, serviceModule);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
