using System;
using Example.Specs.Data;
using ExecutableSpecifications.Core.Watin;
using WatiN.Core;

namespace Example.Specs.Gmail.Pages
{
    public class AboutPage : WatinWebPage
    {

        public Link Gmail
        {
            get { return Get<Link>(text: "Gmail"); }
        }

        public Link ForMobile
        {
            get { return Get<Link>(text: "For mobile"); }
        }

        public Link Features
        {
            get
            {
                return Get<Link>(text: "Features");
            }
        }

        internal Link ForWork
        {
            get { return Get<Link>(text: "For work"); }
        }

        internal Link MoreFeatures
        {
            get { return Get<Link>(text: "More features"); }
        }

        public Link MoreSecure
        {
            get { return Get<Link>(attribute: Tuple.Create("href", "#feature-secure")); }
        }

        internal Div SecureBlurb
        {
            get { return Get<Div>("gweb-lightbox-as-lightbox-fragment"); }
        }

        private const string helpUrlFragment = "/intl/en/mail/help/";
        
        protected override string UrlFragment
        {
            get { return helpUrlFragment + "about.html"; }
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