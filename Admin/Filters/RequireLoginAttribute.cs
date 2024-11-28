using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Filters
{
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["TaiKhoan"] == null &&
                !filterContext.HttpContext.Request.Url.AbsolutePath.Contains("/Admin/Login"))
            {
                filterContext.Result = new RedirectResult("~/Admin/Login");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}