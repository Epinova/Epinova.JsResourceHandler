using Epinova.JsResourceHandler.SampleApp.Models.ViewModels;

namespace Epinova.JsResourceHandler.SampleApp.Business
{
    /// <summary>
    /// Defines a method which may be invoked by PageContextActionFilter allowing controllers
    /// to modify common layout properties of the view model.
    /// </summary>
    interface IModifyLayout
    {
        void ModifyLayout(LayoutModel layoutModel);
    }
}
