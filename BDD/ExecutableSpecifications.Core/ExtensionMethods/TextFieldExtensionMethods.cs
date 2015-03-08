using ExecutableSpecifications.Core.Watin;
using WatiN.Core;

namespace ExecutableSpecifications.Core.ExtensionMethods
{
    internal static class TextFieldExtensionMethods
    {
        private const string DataInputPrefix = "TEST";

        internal static void SetRandomTestValueOfLength(this TextField field, int length = 200)
        {
            field.SetText(DataInputPrefix.RandomAppend(length));
        }
    }
}