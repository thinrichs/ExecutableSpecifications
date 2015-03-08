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


        [Given]
        public void GivenEnglishAboutPageIsLoaded()
        {
            About.SetupPage();
        }

        [When]
        public void WhenIGoToFeatures()
        {
            About.LookAtFeatures();
        }

        [When]
        public void WhenIClickOnMoreFeatures()
        {
            About.LookAtMoreFeatures();
        }

        [Then]
        public void ThenHTTPSSecurityIsMentioned()
        {
            About.ShouldBeMoreSecure();
        }
    }
}