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
            var languageSelector = LanguageSelector.AutoDetect();
            var languageName = languageSelector.Language.Name;
            var filename = context.Request.Path.Substring(Constants.PathBase.Length);

            var debugMode = context.Request.QueryString["debug"] != null;

            var cacheKey = $"{filename}_{languageName}_{(debugMode ? "debug" : "release")}}}";
            var responseObject = context.Cache.Get(cacheKey) as string;

            if (responseObject == null)
            {
                var filePath = GetFullFilePath(filename, context, languageName);
                responseObject = GetJson(filePath, debugMode);
                context.Cache.Insert(cacheKey, responseObject, new CacheDependency(filePath));
            }

            context.Response.Write(responseObject);
            context.Response.ContentType = "text/javascript";
        }

        private static string GetJson(string filePath, bool debugMode)
        {
            var xDocument = GetLanguageFile(filePath);
            if (xDocument == null)
                return null;

            var xElements = xDocument.Root.Elements("language");

            var nodeToSerialize = xElements.Count() == 1 ? xElements.First() : xDocument.Root;
            var serializeXmlNode = JsonConvert.SerializeXNode(nodeToSerialize, debugMode ? Formatting.Indented : Formatting.None, true);

            return $"window.jsl10n = {serializeXmlNode}";
        }

        private static XDocument GetLanguageFile(string filename)
        {
            var file = GetFileStream(filename);
            if (file == null)
                return null;

            var xDocument = XDocument.Load(file);
            file.Close();

            return xDocument;
        }

        private static string GetFullFilePath(string filename, HttpContext context, string languageName)
        {
            string basePath = $"/lang/{filename}.{languageName.ToUpper()}.xml";

            var filePath = context.Server.MapPath(basePath);

            if (!File.Exists(filePath))
                basePath = $"/lang/{filename}.xml";

            filePath = context.Server.MapPath(basePath);

            if (!File.Exists(filePath))
                return null;

            return filePath;
        }

        private static FileStream GetFileStream(string filePath)
        {
            var file = File.OpenRead(filePath);

            return file;
        }

        public bool IsReusable { get; }
    }
}
