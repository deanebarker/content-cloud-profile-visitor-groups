using EPiServer.Personalization.VisitorGroups;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alloy.Liquid.Jackson_Hewitt.Critiera
{
    public interface IExposesOperators
    {
        public List<string> Operators { get; }
    }

    public class OperatorsSelectionFactory<T> : ISelectionFactory where T : IExposesOperators
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            return ((IExposesOperators)Activator.CreateInstance(typeof(T))).Operators.Select(i => new SelectListItem(i, i));
        }
    }
}
