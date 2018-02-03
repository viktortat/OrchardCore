using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace OrchardCore.Mvc.RazorPages
{
    public class ModularPageRouteModelConvention : IPageRouteModelConvention
    {
        private readonly string _pageName;
        private readonly string _route;

        public ModularPageRouteModelConvention(string pageName, string route)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                _pageName = pageName.TrimStart('/');
                _route = route.Trim('/');
            }
        }

        public void Apply(PageRouteModel model)
        {
            if (_pageName == null)
            {
                return;
            }

            if (model.ViewEnginePath.EndsWith('/' + _pageName))
            {
                foreach (var selector in model.Selectors)
                {
                    selector.AttributeRouteModel.SuppressLinkGeneration = true;
                }

                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = _route,
                        Name = _route.Replace('/', '.')
                    }
                });
            }
        }
    }

    public static partial class PageConventionCollectionExtensions
    {
        public static PageConventionCollection AddModularPageRoute(this PageConventionCollection conventions, string pageName, string route)
        {
            conventions.Add(new ModularPageRouteModelConvention(pageName, route));
            return conventions;
        }
    }
}
