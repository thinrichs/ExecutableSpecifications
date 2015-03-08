using System;
using WatiN.Core;

namespace ExecutableSpecifications.Core.Watin
{
    internal static class WatiNExtensions
    {
        internal static void SetText(this TextField textField, string text, bool requiresKeypress = false)
        {
            if (String.IsNullOrEmpty(text))
            {
                return;
            }
            if (requiresKeypress)
            {
                textField.TypeText(text);
            }
            else
            {
                textField.SetAttributeValue("value", text);
            }
        }
    }
}