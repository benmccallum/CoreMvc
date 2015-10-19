using System.Net;
using System.Web;

namespace CoreMvc.Helpers
{
    public class IpAddressHelper
    {
        HttpContext _httpContext;

        public IpAddressHelper(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetUsersIpAddress()
        {
            string ipAddressRaw;
            IPAddress ipAddress;
            var serverVars = _httpContext.Request.ServerVariables;

            var ipList = serverVars["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipList))
            {
                ipAddressRaw = ipList.Split(',')[0];
                if (IPAddress.TryParse(ipAddressRaw, out ipAddress))
                {
                    return ipAddressRaw;
                }
            }

            ipAddressRaw = serverVars["REMOTE_ADDR"];
            if (IPAddress.TryParse(ipAddressRaw, out ipAddress))
            {
                return ipAddressRaw;
            }

            return null;
        }
    }
}
