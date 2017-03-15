using EPiServer.Core;

namespace Epinova.JsResourceHandler.SampleApp.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
