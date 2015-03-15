using Example.Specs.Gmail.DomainSpecificLanguages;
using TechTalk.SpecFlow;

namespace Example.Specs.Gmail.Features.Steps
{
    [Binding]
    public class AboutGMailSteps
    {
        private readonly About _about;

        public AboutGMailSteps()
        {
            _about = new About();
        }

        [BeforeScenario("GMailAbout")]
        internal static void BeforeAdminSessionFeature()
        {
            
            About.SetupSession();
        }

        [AfterScenario("GMailAbout")]
        internal void AfterGmailAbout()
        {
            About.DismantleSession();
        }


        [Given]
        public void GivenEnglishAboutPageIsLoaded()
        {
            _about.SetupPage();
        }

        [When]
        public void WhenIGoToFeatures()
        {
            _about.LookAtFeatures();
        }

        [When]
        public void WhenIReadSecurityInformation()
        {
            _about.LookAtMoreFeatures()
                 .LookAtMoreSecure();
        }

        [Then]
        public void ThenHTTPSSecurityIsMentioned()
        {
            About.ShouldBeMoreSecure();
        }
    }
}