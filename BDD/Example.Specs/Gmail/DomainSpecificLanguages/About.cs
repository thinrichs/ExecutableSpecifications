using Example.Specs.Gmail.Pages;
using ExecutableSpecifications.Core;
using ExecutableSpecifications.Core.Watin;
using FluentAssertions;


namespace Example.Specs.Gmail.DomainSpecificLanguages
{

    internal class About : BaseWatinActions<AuthenticatedWatinSession, AboutPage>
    {
        private const string HttpsIsSecureBlurb = "HTTPS Encryption keeps your mail secure";
        internal static void SetupSession()
        {
            Initialize();
        }


        internal static void SetupPage()
        {
            ProcessAction(LoadTestPage);
        }

        internal static void DismantleSession()
        {
            DisposeSession();
        }

        internal static void LookAtFeatures()
        {
            ProcessAction(() => TestPage.Features.Click());
        }

        internal static void LookAtMoreFeatures()
        {
            ProcessAction(() => TestPage.MoreFeatures.Click());
        }

        internal static void ShouldBeMoreSecure()
        {
            ProcessAssertion(() => TestPage.SecureBlurb.Text.Should().Contain(HttpsIsSecureBlurb));
        }
    }
}