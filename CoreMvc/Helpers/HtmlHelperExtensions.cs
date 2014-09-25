using System;
using System.Web.Mvc;

namespace CoreMvc.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string GetCanonicalUrl(string path)
        {
            path = path.TrimEnd('/').ToLower();
            const string indexDefault = "/index";
            if (path.EndsWith(indexDefault))
            {
                path = path.Substring(0, path.Length - indexDefault.Length);
            }
            return path;
        }

        public static string GetCanonicalUrl(this UrlHelper url, string path = null)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                var rawUrl = url.RequestContext.HttpContext.Request.Url;
                if (rawUrl == null)
                {
                    return null;
                }
                path = String.Format("{0}://{1}{2}", rawUrl.Scheme, rawUrl.Host, rawUrl.AbsolutePath);
            }

            return GetCanonicalUrl(path);
        }

        public static MvcHtmlString CanonicalUrlLink(this HtmlHelper html, string path = null)
        {
            var canonical = new TagBuilder("link");
            canonical.MergeAttribute("rel", "canonical");
            canonical.MergeAttribute("href", GetCanonicalUrl(path));
            return new MvcHtmlString(canonical.ToString(TagRenderMode.SelfClosing));
        }
    }
}
