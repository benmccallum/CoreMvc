﻿using CoreMvc.Sys.Web.Mvc;
using System;
using System.Web;
using System.Web.Mvc;

namespace CoreMvc.Attributes
{
    /// <summary>
    /// A really "basic" implementation of Basic Authorization.
    /// Adapted from: http://cacheandquery.com/blog/2011/03/customizing-asp-net-mvc-basic-authentication/
    /// </summary>
    public class BasicAuthorizeAttribute : AuthorizeAttribute
    {
        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!SecurityProvider.Authenticate(filterContext.HttpContext))
            {
                // HttpBasicUnauthorizedResult inherits from HttpUnauthorizedResult and does the
                // work of displaying the basic authentication prompt to the client
                filterContext.Result = new HttpBasicUnauthorizedResult();
            }
            else
            {
                // AuthorizeCore is in the base class and does the work of checking if we have
                // specified users or roles when we use our attribute
                if (AuthorizeCore(filterContext.HttpContext))
                {
                    var cachePolicy = filterContext.HttpContext.Response.Cache;
                    cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                    cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
                }
                else
                {
                    // auth failed, display login

                    // HttpCustomBasicUnauthorizedResult inherits from HttpUnauthorizedResult and does the
                    // work of displaying the basic authentication prompt to the client
                    filterContext.Result = new HttpBasicUnauthorizedResult();
                }
            }
        }
    }
}
