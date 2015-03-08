using System;
using System.IO;

namespace ExecutableSpecifications.Core.ExtensionMethods
{
    internal static class StringExtensionMethods
    {
        internal static string RandomAppend(this string str, int? length = null)
        {
            var result = Path.GetRandomFileName().Replace(".", "");
            var startIndex = result.Length - Math.Min(length.GetValueOrDefault(), result.Length);

            startIndex += str.Length;
            return str + result.Substring(startIndex);
        }
    }
}