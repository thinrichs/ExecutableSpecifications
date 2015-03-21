using System;
using Example.Specs.Data;
using ExecutableSpecifications.Core.Watin;
using WatiN.Core;

namespace Example.Specs.Gmail.Pages
{
    public class AboutPage : WatinWebPage
    {
        public Link Features
        {
            get { return Get<Link>(text: "Features"); }
        }

        internal Link MoreFeatures
        {
            get { return Get<Link>(text: "More features"); }
        }

        internal Link Mobile
        {
            get { return Get<Link>(text: "For mobile"); }
        }

        public Link MoreSecure
        {
            get { return Get<Link>(attribute: Tuple.Create("data-g-action", "Gmail now even more secure")); }
        }

        protected override string UrlFragment
        {
            get { return "/intl/en/mail/help/about.html"; }
        }

        protected override string PageTitleSegment
        {
            get { return "Free Storage and Email"; }
        }

        public override string Server
        {
            get { return TestData.Hosts.GMail; }
        }
    }
}