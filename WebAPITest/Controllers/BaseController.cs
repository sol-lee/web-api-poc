using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPITest.Controllers
{
    public abstract class BaseController : Controller
    {
        public const string URLHELPER = "UrlHelper";

        /// <summary>
        /// The purpose of this method is to inject "Url" into HTTP Context.
        /// And the reason we didn't use constructor is because HttpContext is not ready while class
        /// just been created. It's surely ready while action been called.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            context.HttpContext.Items[URLHELPER] = this.Url;
        }
    }
}
