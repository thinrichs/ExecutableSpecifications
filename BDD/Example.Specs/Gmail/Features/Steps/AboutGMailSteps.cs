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

        [Given(@"English About page is loaded")]
        public void GivenEnglishAboutPageIsLoaded()
        {
            _about.SetupPage();
        }

        [When(@"I go to `Features`")]
        public void WhenIGoToFeatures()
        {
            _about.LookAtFeatures();
        }

        [When(@"I read security information")]
        public void WhenIReadSecurityInformation()
        {
              _about.LookAtMoreFeatures()
                 .LookAtMoreSecure();
        }

        [Then(@"HTTPS Security is mentioned")]
        public void ThenHTTPSSecurityIsMentioned()
        {
            About.ShouldBeMoreSecure();
        }

        [When(@"I go to `For Mobile`")]
        public void WhenIGoToForMobile()
        {
            _about.LookAtMobile();
        }

    }
}