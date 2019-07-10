using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;

namespace WEBPOS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            //CultureInfo cInf = new CultureInfo("es-ES", false);
            //// NOTE: change the culture name en-ZA to whatever culture suits your needs

            //cInf.DateTimeFormat.DateSeparator = "/";
            //cInf.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //cInf.DateTimeFormat.LongDatePattern = "dd/MM/yyyy hh:mm:ss tt";

            //System.Threading.Thread.CurrentThread.CurrentCulture = cInf;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = cInf;
        }
    }
}
