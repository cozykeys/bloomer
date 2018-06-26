namespace KbUtil.Lib.SvgGeneration.Internal
{
    using KbUtil.Lib.Models.Keyboard;
    using System;

    internal static class Util
    {
        public static string GetKeyboardConstant(Element element, string name)
        {
            if (element.GetType() != typeof(Keyboard))
            {
                if (element.Parent == null)
                {
                    throw new InvalidOperationException();
                }

                return GetKeyboardConstant(element.Parent, name);
            }

            return ((Keyboard)element).Constants[name].Value;
        }
    }
}
