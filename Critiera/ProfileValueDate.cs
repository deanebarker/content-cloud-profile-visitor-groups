using Alloy.Liquid.Jackson_Hewitt.Critiera;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using JacksonHewitt;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Criteria
{
    public class ProfileValueDateSettings : CriterionModelBase, IValidateCriterionModel
    {
        [Required]
        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/key"
        )]
        public string Key { get; set; }

        [CriterionPropertyEditor(
            SelectionFactoryType = typeof(OperatorsSelectionFactory<ProfileValueDate>),
            LabelTranslationKey = "/visitorgroups/profile/criteria/operator"
        )]
        public string Operator { get; set; }

        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/value"
        )]
        public DateTime Value { get; set; }

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
        DisplayName = "Profile Value: Date",
        Description = "Compare a date value from the visitor's profile"
    )]
    public class ProfileValueDate : CriterionBase<ProfileValueDateSettings>, IExposesOperators
    {
        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            var profileManager = ServiceLocator.Current.GetInstance<IProfileManager>();
            var fromProfile = profileManager.GetDate(Model.Key);
            if (fromProfile == null) return false;

            var fromCriteria = DateOnly.FromDateTime(Model.Value);

            return Comparisons[Model.Operator](fromProfile.Value, fromCriteria);
        }

        public Dictionary<string, Func<DateOnly, DateOnly, bool>> Comparisons { get; } = new()
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

}