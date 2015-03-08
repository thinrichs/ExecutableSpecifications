using Example.Specs.Gmail.DomainSpecificLanguages;
using TechTalk.SpecFlow;

namespace Example.Specs.Gmail.Features.Steps
{
    [Binding]
    public class AboutGMailSteps
    {
        [BeforeScenario("GMailAbout")]
        internal static void BeforeAdminSessionFeature()
        {
            About.SetupSession();
        }

        [AfterScenario("GMailAbout")]
        internal static void AfterParmMaster()
        {
            About.DismantleSession();
        }


        [Given(@"English About page is loaded")]
        public void GivenEnglishAboutPageIsLoaded()
        {
            About.SetupPage();
        }

        [When(@"I go to features")]
        public void WhenIGoToFeatures()
        {
            About.LookAtFeatures();
        }

        [When(@"I click on More Features")]
        public void WhenIClickOnMoreFeatures()
        {
            About.LookAtMoreFeatures();
        }

        [Then(@"HTTPS Security is mentioned")]
        public void ThenSecurityIsAFeatureInTheExpansion()
        {
            About.ShouldBeMoreSecure();
        }
    }
}