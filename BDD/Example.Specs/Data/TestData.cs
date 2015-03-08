using ExecutableSpecifications.Core;

namespace Example.Specs.Data
{
    internal static class TestData
    {
        internal static dynamic Hosts
        {
            get { return DataManager.GetDynamicByLocalName("Hosts"); }
        }

        internal static dynamic GMail
        {
            get { return DataManager.GetDynamicByLocalName("GMail"); }
        }

        internal static dynamic Reddit
        {
            get { return DataManager.GetDynamicByLocalName("Reddit"); }
        }
    }
}