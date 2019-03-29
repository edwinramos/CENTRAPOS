using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.Models;

namespace WEBPOS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserModel, DeUser>();
                cfg.CreateMap<DeUser, UserModel>();
                cfg.CreateMap<DeItem, ItemModel>();
                cfg.CreateMap<ItemModel, DeItem>();
            });
        }
    }
}
