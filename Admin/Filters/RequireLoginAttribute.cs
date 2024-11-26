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
            // Kiểm tra nếu người dùng chưa đăng nhập và không ở trang đăng nhập
            if (HttpContext.Current.Session["TaiKhoan"] == null &&
                !filterContext.HttpContext.Request.Url.AbsolutePath.Contains("/Admin/Login"))
            {
                // Nếu chưa đăng nhập và không ở trang đăng nhập, chuyển hướng đến trang đăng nhập
                filterContext.Result = new RedirectResult("~/Admin/Login");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}