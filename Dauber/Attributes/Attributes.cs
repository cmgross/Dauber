using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using RequireHttpsAttributeBase = System.Web.Mvc.RequireHttpsAttribute;

namespace Dauber.Attributes
{
    public class IsAdminAttribute : AuthorizeAttribute
    {
        private bool _authorized;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            _authorized = base.AuthorizeCore(httpContext);
            if (!_authorized) return _authorized;
            _authorized = Cacheable.IsAdmin(httpContext.User.Identity.Name);
            return _authorized;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!_authorized)
                filterContext.Result = new ViewResult { ViewName = "NotAuthorized" };
        }
    }

    public class RequireHttpsAttribute : RequireHttpsAttributeBase
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsSecureConnection)
            {
                return;
            }

            if (string.Equals(filterContext.HttpContext.Request.Headers["X-Forwarded-Proto"],
                "https",
                StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }

            HandleNonHttpsRequest(filterContext);
        }
    }
}