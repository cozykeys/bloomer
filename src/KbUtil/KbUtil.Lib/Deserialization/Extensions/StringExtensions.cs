namespace KbUtil.Lib.Deserialization.Extensions
{
    using KbUtil.Lib.Models.Keyboard;
    using System;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        private static readonly Regex _constantRegex = new Regex(@"^.*(\$\{(.*)\}).*$");

        public static string ReplaceConstants(this string str, Element element)
        {
            Match match = _constantRegex.Match(str);

            if (match.Success && match.Groups.Count == 3)
            {
                // This is the entire token (I.E. "${MyVariable}")
                string constantToken = match.Groups[1].Value;

                // This is just the name in the token (I.E. "MyVariable")
                string constantName = match.Groups[2].Value;

                string constantValue = GetKeyboardConstant(element, constantName).Value;

                str = str.Replace(constantToken, constantValue);
            }

            return str;
        }

        public static int ToInt(this string str) => int.Parse(str);
        public static float ToFloat(this string str) => float.Parse(str);
        public static bool ToBool(this string str) => bool.Parse(str);
        public static Version ToVersion(this string str) => Version.Parse(str);

        private static Constant GetKeyboardConstant(Element element, string name)
        {
            if (element == null)
            {
                throw new Exception($"Failed to find a constant named \"{name}\" in the scope of the element.");
            }

            if (element.Constants != null && element.Constants.ContainsKey(name))
            {
                return element.Constants[name];
            }

            return GetKeyboardConstant(element.Parent, name);
        }
    }
}
