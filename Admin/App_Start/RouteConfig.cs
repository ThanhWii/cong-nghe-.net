using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Loại bỏ route mặc định cho các tệp .axd
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Route cho trang đăng nhập trong AdminController
            routes.MapRoute(
                name: "AdminLogin",
                url: "Admin/Login",
                defaults: new { controller = "Admin", action = "Login" }
            );

            // Route mặc định cho controller Admin và action Login nếu chưa đăng nhập
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Login", id = UrlParameter.Optional }
            );

            // Route cho các controller khác (Home)
            routes.MapRoute(
                name: "Home",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
