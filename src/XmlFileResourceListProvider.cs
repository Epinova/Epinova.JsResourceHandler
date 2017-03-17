using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Epinova.JsResourceHandler
{
    public class XmlFileResourceListProvider : IResourceListProvider
    {
        private string _filePath;

        public string GetJson(string filename, HttpContext context, string languageName, bool debugMode)
        {
            _filePath = GetFullFilePath(filename, context, languageName);

            var xDocument = GetLanguageFile(_filePath);
            if(xDocument == null)
                return null;

            var xElements = xDocument.Root.Elements("language");

            var nodeToSerialize = xElements.Count() == 1 ? xElements.First() : xDocument.Root;
            return JsonConvert.SerializeXNode(nodeToSerialize, debugMode ? Formatting.Indented : Formatting.None, true);
        }

        public CacheDependency GetCacheDependency()
        {
            return new CacheDependency(_filePath);
        }

        private static XDocument GetLanguageFile(string filename)
        {
            var file = GetFileStream(filename);
            if(file == null)
                return null;

            var xDocument = XDocument.Load(file);
            file.Close();

            return xDocument;
        }

        private static FileStream GetFileStream(string filePath)
        {
            var file = File.OpenRead(filePath);

            return file;
        }

        private static string GetFullFilePath(string filename, HttpContext context, string languageName)
        {
            string basePath = $"/Resources/LanguageFiles/{filename}.{languageName.ToUpper()}.xml";

            var filePath = context.Server.MapPath(basePath);

            if(!File.Exists(filePath))
                basePath = $"/Resources/LanguageFiles/{filename}.xml";

            filePath = context.Server.MapPath(basePath);

            if(!File.Exists(filePath))
                return null;

            return filePath;
        }
    }
}
