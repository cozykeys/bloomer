using System.Collections.Generic;
using System.Linq;

namespace KbUtil.Lib.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToCssStyleString(this IDictionary<string, string> dictionary)
            => string.Join(";", dictionary.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
    }
}
