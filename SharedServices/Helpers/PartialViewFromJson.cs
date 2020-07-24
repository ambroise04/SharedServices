using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SharedServices.UI.Controllers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SharedServices.UI.Helpers
{
    public class PartialViewFromJson
    {
        private ICompositeViewEngine _viewEngine;

        public PartialViewFromJson(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
        }

        public async Task<string> PartialView(RequestController controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw, new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}