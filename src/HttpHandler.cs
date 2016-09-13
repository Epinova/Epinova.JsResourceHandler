using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using EPiServer.Core;
using Newtonsoft.Json;

namespace Epinova.JsResourceHandler
{
    public class HttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            LanguageSelector languageSelector = LanguageSelector.AutoDetect();
            var languageName = languageSelector.Language.Name;
            string filename = context.Request.Path.Substring(Constants.PathBase.Length);

            bool debugMode = context.Request.QueryString["debug"] != null;

            string cacheKey = $"{filename}_{languageName}_{(debugMode ? "debug" : "release")}}}";
            string responseObject = context.Cache.Get(cacheKey) as string;

            if (responseObject == null)
            {
                responseObject = GetJson(context, filename, languageName, debugMode);
                context.Cache.Insert(cacheKey, responseObject, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 2, 0, 0));
            }

            context.Response.Write(responseObject);
            context.Response.ContentType = "text/javascript";
        }

        private static string GetJson(HttpContext context, string filename, string languageName, bool debugMode)
        {
            XDocument xDocument = GetLanguageFile(filename, context, languageName);
            if (xDocument == null)
                return null;

            IEnumerable<XElement> xElements = xDocument.Root.Elements("language");

            var nodeToSerialize = xElements.Count() == 1 ? xElements.First() : xDocument.Root;

            string serializeXmlNode = JsonConvert.SerializeXNode(nodeToSerialize, debugMode ? Formatting.Indented : Formatting.None, true);
            return $"window.jsl10n = {serializeXmlNode}";
        }

        private static XDocument GetLanguageFile(string filename, HttpContext context, string languageName)
        {
            var file = GetFileStream(filename, context, languageName);
            if (file == null)
                return null;

            var xDocument = XDocument.Load(file);
            file.Close();
            return xDocument;
        }

        private static FileStream GetFileStream(string filename, HttpContext context, string languageName)
        {
            string basePath = $"/lang/{filename}.{languageName.ToUpper()}.xml";

            var filePath = context.Server.MapPath(basePath);

            if (!File.Exists(filePath))
                filePath = $"/lang/{filename}.xml";

            if (!File.Exists(filePath))
                return null;

            FileStream file = File.OpenRead(filePath);

            return file;
        }

        public bool IsReusable { get; }
    }
}
