using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AudioPlayer.Controllers
{
    public class CustomController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewData["DarkTheme"] = Convert.ToBoolean(HttpContext.Request.Cookies["DarkTheme"] ?? "False");

            base.OnActionExecuted(context);
        }
    }
}
