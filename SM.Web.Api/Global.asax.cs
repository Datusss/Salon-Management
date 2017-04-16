using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using SM.Persistence;
using SM.Services.Impl;

namespace SM.Web.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Init_Container();

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration
             .Formatters
             .JsonFormatter
             .SerializerSettings
             .DefaultValueHandling
                            = Newtonsoft.Json.DefaultValueHandling.Include;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }

        protected void Init_Container()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly).PropertiesAutowired();
            builder.Register(o => new ThaiAnhSalonEntities()).InstancePerRequest();

            var assembly = Assembly.GetAssembly(typeof(UserService));
            builder.RegisterAssemblyTypes(assembly).Where(o => o.Namespace != null && o.Namespace.EndsWith("Services.Impl")).AsImplementedInterfaces();

            var container = builder.Build();
            var apiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = apiResolver;

        }
    }
}
