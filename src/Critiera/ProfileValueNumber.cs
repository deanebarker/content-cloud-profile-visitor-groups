using Alloy.Liquid.Jackson_Hewitt.Critiera;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Criteria
{

    public class ProfileValueNumberSettings : CriterionModelBase, IValidateCriterionModel
    {
        [Required]
        [CriterionPropertyEditor(
            LabelTranslationKey = "/visitorgroups/profile/criteria/key"
        )]
        public string Key { get; set; }

        [CriterionPropertyEditor(
            SelectionFactoryType = typeof(OperatorsSelectionFactory<ProfileValueNumber>),
            LabelTranslationKey = "/visitorgroups/profile/criteria/operator"
        )]

        [Required]
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
        DisplayName = "Profile Value: Number",
        Description = "Compare a number value from the visitor's profile"
    )]
    public class ProfileValueNumber : CriterionBase<ProfileValueNumberSettings>, IExposesOperators
    {
        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            var profileManager = ServiceLocator.Current.GetInstance<IProfileManager>();
            var number = profileManager.GetInt(Model.Key);
            if(number == null) return false; // Wasn't a number...

            return Comparisons[Model.Operator](Model.Value, number.Value);
        }

        public Dictionary<string, Func<int, int, bool>> Comparisons { get; } = new()
        {
            [">"] = (setting, fromProfile) => { return fromProfile > setting; },
            ["<"] = (setting, fromProfile) => { return fromProfile < setting; },
            [">="] = (setting, fromProfile) => { return fromProfile >= setting; },
            ["<="] = (setting, fromProfile) => { return fromProfile <= setting; },
            ["="] = (setting, fromProfile) => { return fromProfile == setting; },
            ["<>"] = (setting, fromProfile) => { return fromProfile != setting; }
        };

        public List<string> Operators => Comparisons.Keys.ToList();
    }
}