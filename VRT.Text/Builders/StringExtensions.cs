using System.Collections.Generic;
using VRT.Text.TextBuilders;

namespace VRT.Text.Builders
{   
    internal static class StringExtensions
    {
        /// <summary>
        /// Method translates templateText into TextComponent (composite)
        /// </summary>
        /// <param name="templateText">Template text i.e. Hello {Name} </param>
        /// <returns>TextComponent able to create string based on provided placeholder values</returns>
        internal static TextComponent AsTextComposite(this string templateText)
        {
            if (string.IsNullOrWhiteSpace(templateText) || templateText.Length < 2)
            {
                return new StaticTextComponent(templateText);                
            }

            var staticText = new List<char>(templateText.Length);
            var tokenText = new List<char>(templateText.Length);
            var len = templateText.Length;

            var root = TextComponent.Empty;

            for (var i = 0; i < len; i++)
            {
                var curChar = templateText[i];

                if ((curChar == '{' || curChar == '}') && i < len - 1 && templateText[i + 1] == curChar)
                {
                    staticText.Add(curChar);
                    i++;
                    continue;
                }

                if (curChar != '{' || i >= len - 1)
                {
                    staticText.Add(curChar);
                    continue;
                }

                tokenText.Clear();
                for (var j = i + 1; j < templateText.Length; j++)
                {
                    var chr = templateText[j];
                    if (chr == '{')
                    {
                        staticText.Add(chr);
                        staticText.AddRange(tokenText);
                        i = j - 1;
                        tokenText.Clear();
                        break;
                    }

                    if (chr == '}')
                    {
                        i = j;
                        break;
                    }
                    tokenText.Add(chr);
                }
                if (tokenText.Count <= 0) continue;
                if (staticText.Count > 0)
                {
                    root.Add(new StaticTextComponent(new string(staticText.ToArray())));
                    staticText.Clear();
                }
                root.Add(new PlaceholderComponent(new string(tokenText.ToArray())));
            }
            if (staticText.Count > 0)
            {
                root.Add(new StaticTextComponent(new string(staticText.ToArray())));
            }
            return root;
        }        
    }
}
