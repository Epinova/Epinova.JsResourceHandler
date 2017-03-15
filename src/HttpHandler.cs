using System;
using System.Web;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Epinova.JsResourceHandler
{
    public class HttpHandler : IHttpHandler
    {
        private Injected<IResourceListProvider> _provider;

        public void ProcessRequest(HttpContext context)
        {
            if(_provider.Service == null)
                throw new InvalidOperationException("Implementation of `IResourceListProvider` is not configured in IoC container.");

            var languageSelector = LanguageSelector.AutoDetect();
            var languageName = languageSelector.Language.Name;
            var filename = context.Request.Path.Substring(Constants.PathBase.Length);

            var debugMode = context.Request.QueryString["debug"] != null;

            var cacheKey = $"{filename}_{languageName}_{(debugMode ? "debug" : "release")}}}";
            var responseObject = context.Cache.Get(cacheKey) as string;

            if(responseObject == null)
            {
                responseObject = _provider.Service.GetJson(filename, context, languageName, debugMode);

                context.Cache.Insert(cacheKey, responseObject, _provider.Service.GetCacheDependency());
            }

            context.Response.Write(responseObject);
            context.Response.ContentType = "text/javascript";
        }

        public bool IsReusable { get; }
    }
}
