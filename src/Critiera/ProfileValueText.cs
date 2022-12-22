using Alloy.Liquid.Jackson_Hewitt.Critiera;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Criteria
{
    public class ProfileValueTextSettings : CriterionModelBase, IValidateCriterionModel
    {
        [Required]
        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/key"
        )]
        public string Key { get; set; }

        [CriterionPropertyEditor(
            SelectionFactoryType = typeof(OperatorsSelectionFactory<ProfileValueText>),
            LabelTranslationKey = "/visitorgroups/profile/criteria/operator"
        )]

        [Required]
        public string Operator { get; set; }

        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/value"
        )]
        public string Value { get; set; }

        [Display(Name = "Case Sensitive")]
        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/text/casing"
        )]
        public bool MatchCase { get; set; }

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
        DisplayName = "Profile Value: Text",
        Description = "Compare a text value from the visitor's profile"
    )]
    public class ProfileValueText : CriterionBase<ProfileValueTextSettings>, IExposesOperators
    {
        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            var profileManager = ServiceLocator.Current.GetInstance<IProfileManager>();
            var value = profileManager.GetString(Model.Key);
            if(value == null) return false;

            var fromProfile = Model.Value;

            if(!Model.MatchCase)
            {
                value = value.ToLower();
                fromProfile= fromProfile.ToLower();
            }

            return Comparisons[Model.Operator](fromProfile.Trim(), value.Trim());
        }

        public Dictionary<string, Func<string, string, bool>> Comparisons { get; } = new()
        {
            ["equals"] = (setting, fromProfile) => { return fromProfile == setting; },
            ["contains"] = (setting, fromProfile) => { return fromProfile.Contains(setting); },
            ["starts with"] = (setting, fromProfile) => { return fromProfile.StartsWith(setting); },
            ["ends with"] = (setting, fromProfile) => { return fromProfile.EndsWith(setting); },
            ["is empty"] = (setting, fromProfile) => { return string.IsNullOrWhiteSpace(fromProfile); },
            ["is not empty"] = (setting, fromProfile) => { return !string.IsNullOrWhiteSpace(fromProfile); },
            ["is of pattern"] = (setting, fromProfile) => { return Regex.IsMatch(fromProfile, setting); }
        };

        public List<string> Operators => Comparisons.Keys.ToList();
    }

}