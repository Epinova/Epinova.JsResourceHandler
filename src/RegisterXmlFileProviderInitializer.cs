using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Epinova.JsResourceHandler
{
    [InitializableModule]
    public class RegisterXmlFileProviderInitializer : IConfigurableModule
    {
        public void Initialize(InitializationEngine context) { }

        public void Uninitialize(InitializationEngine context) { }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IResourceListProvider,  XmlFileResourceListProvider>();
        }
    }
}
