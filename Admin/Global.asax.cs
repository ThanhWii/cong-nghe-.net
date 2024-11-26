using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Kiểm tra HttpContext và Session có null không
            if (HttpContext.Current != null &&
                HttpContext.Current.Session != null &&
                HttpContext.Current.Request != null &&
                HttpContext.Current.Request.Url != null)
            {
                // Kiểm tra nếu người dùng chưa đăng nhập và không ở trang đăng nhập
                if (HttpContext.Current.Session["TaiKhoan"] == null &&
                    !HttpContext.Current.Request.Url.AbsolutePath.Contains("/Admin/Login"))
                {
                    // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                    Response.Redirect("~/Admin/Login");
                }
            }
        }
    }
}
