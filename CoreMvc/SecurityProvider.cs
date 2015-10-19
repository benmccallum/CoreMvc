using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace CoreMvc
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BasicScheme = "Basic";

        /// <summary>
        /// 
        /// </summary>
        public const string ChallengeAuthenticationHeaderName = "WWW-Authenticate";

        /// <summary>
        /// 
        /// </summary>
        public const char AuthorizationHeaderSeparator = ':';

        /// <summary>
        /// 
        /// </summary>
        public const string AdministratorRole = "Administrator";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool Authenticate(HttpContextBase context)
        {
            if (!context.Request.Headers.AllKeys.Contains("Authorization"))
            {
                return false;
            }
            return Authenticate(context.Request.Headers["Authorization"]);
        }

        public static bool Authenticate(string authHeader)
        {
            IPrincipal principal;
            if (TryGetPrincipal(authHeader, out principal))
            {
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authHeader"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static bool TryGetPrincipal(string authHeader, out IPrincipal principal)
        {
            var creds = ParseAuthHeader(authHeader);
            if (creds != null)
            {
                if (TryGetPrincipal(creds[0], creds[1], out principal)) return true;
            }

            principal = null;
            return false;
        }

        private static string[] ParseAuthHeader(string authHeader)
        {
            // Check this is a Basic Auth header
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith(BasicScheme))
            {
                return null;
            }

            // Pull out the Credentials with are seperated by ':' and Base64 encoded
            string base64Credentials = authHeader.Substring(6);
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(base64Credentials)).Split(new[] { AuthorizationHeaderSeparator });

            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[0])) return null;

            // Okay this is the credentials
            return credentials;
        }

        private static bool TryGetPrincipal(string userName, string password, out IPrincipal principal)
        {
            if (userName == WebConfigurationManager.AppSettings["CoreMvc.AdminUsername"] && password == ConfigurationManager.AppSettings["CoreMvc.AdminPassword"])
            {
                // once the user is verified, assign it to an IPrincipal with the identity name and applicable roles
                principal = new GenericPrincipal(new GenericIdentity(userName), new[] { AdministratorRole });
                return true;
            }
            principal = null;
            return false;
        }
    }
}
