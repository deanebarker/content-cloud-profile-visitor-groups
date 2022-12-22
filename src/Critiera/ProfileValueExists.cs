using Alloy.Liquid.Jackson_Hewitt.Critiera;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Criteria
{
    public class ProfileValueExistsSettings : CriterionModelBase, IValidateCriterionModel
    {
        [Required]
        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/key"
        )]
        public string Key { get; set; }

        [CriterionPropertyEditor(
            SelectionFactoryType = typeof(OperatorsSelectionFactory<ProfileValueExists>),
            LabelTranslationKey = "/visitorgroups/profile/criteria/operator"
        )]
        public string Operator { get; set; }

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
        DisplayName = "Profile Value: Exists",
        Description = "Check that a value from the profile exists or not"
    )]
    public class ProfileValueExists : CriterionBase<ProfileValueExistsSettings>, IExposesOperators
    {
        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            var profileManager = ServiceLocator.Current.GetInstance<IProfileManager>();
            var value = profileManager.GetString(Model.Key);

            return Comparisons[Model.Operator](value);
        }

        public Dictionary<string, Func<string, bool>> Comparisons { get; } = new()
        {
            ["exists"] = (fromProfile) => { return fromProfile != null; },
            ["does not exist"] = (fromProfile) => { return fromProfile == null; },
        };

        public List<string> Operators => Comparisons.Keys.ToList();
    }

}