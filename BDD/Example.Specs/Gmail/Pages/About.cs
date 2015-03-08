using Example.Specs.Data;
using ExecutableSpecifications.Core.Watin;
using WatiN.Core;
using ExecutableSpecifications.Core;

namespace Example.Specs.Gmail.Pages
{
    public class AboutPage : WatinWebPage
    {
        
        public ListItem Gmail
        {
            get { return Get<ListItem>(value: "Gmail"); }
        }

        public ListItem ForMobile
        {
            get { return Get<ListItem>(value: "For mobile"); }
        }

        public ListItem Features
        {
            get { return Get<ListItem>(value: "Features"); }
        }

        internal ListItem ForWork
        {
            get { return Get<ListItem>(value: "For work"); }
        }

        internal Link MoreFeatures
        {
            get { return Get<Link>(value: "More Features"); }
        }

        internal Para SecureBlurb
        {
            get { return Get<Para>(selector: "p[id='gweb-lightbox-as-lightbox-fragment']"); }
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
            get { return TestData.Hosts.Gmail; }

        }
    }
}