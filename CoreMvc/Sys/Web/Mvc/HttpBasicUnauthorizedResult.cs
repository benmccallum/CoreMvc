using CoreMvc;
using System;
using System.Web.Mvc;

namespace CoreMvc.Sys.Web.Mvc
{
    /// <summary>
    /// A really "basic" extension of the HttpUnauthorizedResult that challenges the user for
    /// their Basic Authentication credentials by adding the appropriate response headers.
    /// 
    /// Adapted from: http://cacheandquery.com/blog/2011/03/customizing-asp-net-mvc-basic-authentication/
    /// </summary>
    public class HttpBasicUnauthorizedResult : HttpUnauthorizedResult
    {
        // the base class already assigns the 401.
        // we bring these constructors with us to allow setting status text

        /// <summary>
        /// 
        /// </summary>
        public HttpBasicUnauthorizedResult()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusDescription"></param>
        public HttpBasicUnauthorizedResult(string statusDescription)
            : base(statusDescription)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // this is really the key to bringing up the basic authentication login prompt.
            // this header is what tells the client we need basic authentication
            context.HttpContext.Response.AddHeader(SecurityProvider.ChallengeAuthenticationHeaderName, SecurityProvider.BasicScheme);
            base.ExecuteResult(context);
        }
    }
}
