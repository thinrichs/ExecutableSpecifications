using Example.Specs.Gmail.Pages;
using ExecutableSpecifications.Core;
using ExecutableSpecifications.Core.Watin;
using FluentAssertions;
using WatiN.Core;


namespace Example.Specs.Gmail.DomainSpecificLanguages
{

    internal class About : BaseWatinActions<AuthenticatedWatinSession, AboutPage>
    {
        private const string HttpsIsSecureBlurb = "HTTPS encryption keeps your mail secure";
        internal static void SetupSession()
        {
            Initialize();
        }
        internal static void DismantleSession()
        {
            DisposeSession();
        }

        internal About SetupPage()
        {
            return PerformAction<About>(LoadTestPage);
        }


        internal About LookAtFeatures()
        {
            return PerformAction<About>(() => TestPage.Features.Click());
        }

        internal About LookAtMoreFeatures()
        {
            return PerformAction<About>(() => TestPage.MoreFeatures.Click());
        }

        internal static void ShouldBeMoreSecure()
        {
            ProcessAssertion(() => TestPage.MoreSecure.Parent.
                Text.Should().Contain(HttpsIsSecureBlurb));
        }

        internal About LookAtMoreSecure()
        {
            return PerformAction<About>(() => TestPage.MoreSecure.Click());
        }
    }
}