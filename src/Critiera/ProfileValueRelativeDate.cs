using Alloy.Liquid.Jackson_Hewitt.Critiera;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Criteria
{
    public class ProfileValueRelativeDateSettings : CriterionModelBase, IValidateCriterionModel
    {
        [Required]
        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/key"
        )]
        public string Key { get; set; }

        [Required]
        [CriterionPropertyEditor(
            SelectionFactoryType = typeof(DatePartSelectionFactory),
            LabelTranslationKey = "/visitorgroups/profile/criteria/relativedate/datepart"
        )]
        public DatePart DatePart { get; set; }

        [CriterionPropertyEditor(
            SelectionFactoryType = typeof(OperatorsSelectionFactory<ProfileValueRelativeDate>),
            LabelTranslationKey = "/visitorgroups/profile/criteria/operator"
        )]
        public string Operator { get; set; }

        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/value"
        )]
        public int Value { get; set; }

        public override ICriterionModel Copy()
        {
            return ShallowCopy();
        }

        public CriterionValidationResult Validate(VisitorGroup currentGroup)
        {
            return new CriterionValidationResult(true);
        }
    }

    [VisitorGroupCriterion(
        Category = "Profile",
        DisplayName = "Profile Value: Relative Date",
        Description = "Compare a value derived from the elapsed time between now and a date from the visitor's profile"
    )]
    public class ProfileValueRelativeDate : CriterionBase<ProfileValueRelativeDateSettings>, IExposesOperators
    {
        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            var profileManager = ServiceLocator.Current.GetInstance<IProfileManager>();
            var fromProfile = profileManager.GetDate(Model.Key);
            if (fromProfile == null) return false;

            var timespan = DateTime.Now - fromProfile.Value.ToDateTime(TimeOnly.MinValue);

            var value = Model.DatePart switch
            {
                DatePart.Years => (int)timespan.TotalDays / 365,
                DatePart.Months => (int)timespan.TotalDays / 12,
                DatePart.Weeks => (int)timespan.TotalDays / 7,
                _ => (int)timespan.TotalDays
            };

            var fromCriteria = Model.Value;

            return Comparisons[Model.Operator](value, fromCriteria);
        }

        public Dictionary<string, Func<int, int, bool>> Comparisons { get; } = new()
        {
            [">"] = (fromProfile, fromCriteria) => { return fromProfile > fromCriteria; },
            ["<"] = (fromProfile, fromCriteria) => { return fromProfile < fromCriteria; },
            [">="] = (fromProfile, fromCriteria) => { return fromProfile >= fromCriteria; },
            ["<="] = (fromProfile, fromCriteria) => { return fromProfile <= fromCriteria; },
            ["="] = (fromProfile, fromCriteria) => { return fromProfile == fromCriteria; },
            ["<>"] = (fromProfile, fromCriteria) => { return fromProfile != fromCriteria; }
        };

        public List<string> Operators => Comparisons.Keys.ToList();
    }

    public enum DatePart
    {
        Years,
        Months,
        Weeks,
        Days
    }

    public class DatePartSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            var options = new List<SelectListItem>();
            foreach(var value in Enum.GetValues(typeof(DatePart)))
            {
                options.Add(new SelectListItem(value.ToString(), value.ToString()));
            }
            return options;
        }
    }
}