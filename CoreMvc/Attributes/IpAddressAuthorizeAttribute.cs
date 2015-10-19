using CoreMvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CoreMvc.Attributes
{
    public class IpAddressAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly IList<string> _ips;

        private readonly IpAddressHelper _ipAddressHelper;

        /// <summary>
        /// TODO: Make this CoreWeb.IWebConfigurationManager 
        /// </summary>
        //private readonly WebConfigurationManager _webConfigurationManager;

        public IpAddressAuthorizeAttribute()
            : this(new IpAddressHelper(HttpContext.Current))
        {

        }

        public IpAddressAuthorizeAttribute(IpAddressHelper ipAddressHelper)//IWebConfigurationManager webConfigurationManager)
        {
            //_webConfigurationManager = webConfigurationManager;
            _ipAddressHelper = ipAddressHelper;

            const string appSettingKey = "CoreMvc.AuthorizedIpAddresses";

            var ipsRaw = WebConfigurationManager.AppSettings[appSettingKey];
            if (string.IsNullOrWhiteSpace(ipsRaw))
            {
                throw new InvalidProgramException(string.Format("Missing appSetting {0} for IpAddressAuthorizeAttribute to function. Please add it. The format should be piped (|) separated ip addresses.", appSettingKey));
            }

            _ips = ipsRaw.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsLocal)
            {
                // Don't auth local requests
                return true;
            }

            var ipAddress = _ipAddressHelper.GetUsersIpAddress();

            return _ips.Contains(ipAddress);
        }
    }
}
