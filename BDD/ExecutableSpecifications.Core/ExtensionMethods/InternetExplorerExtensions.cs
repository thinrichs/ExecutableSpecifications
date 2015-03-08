using ExecutableSpecifications.Core.Watin;
using WatiN.Core;

namespace ExecutableSpecifications.Core.ExtensionMethods
{
    internal static class InternetExplorerExtensions
    {
        internal static TPage Load<TPage>(this IE browser, TPage page, bool validateTitleSegment = false, string url = "") where TPage : WatinWebPage
        {
            page.Load(browser, validateTitleSegment, url);
            return page;
        }
    }
}