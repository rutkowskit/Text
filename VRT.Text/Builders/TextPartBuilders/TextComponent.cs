using System.Collections.Generic;
using System.Text;

namespace VRT.Text.TextBuilders
{
    /// <summary>
    /// Abstract text component class
    /// </summary>
    internal abstract class TextComponent    
    {
        public static TextComponent Empty => new StaticTextComponent(null);

        private readonly List<TextComponent> _subParts;
        protected TextComponent()
        {
            _subParts = new List<TextComponent>();
        }        
        public void Add(TextComponent textPart)
        {            
            _subParts.Add(textPart);
        }
        public string BuildText(IReadOnlyDictionary<string, string> values, 
            bool preserveUnresolvedPlaceholderNames)
        {
            if(_subParts.Count==0) //process only self if leaf
                return BuildTextPart(values, preserveUnresolvedPlaceholderNames);

            var result = new StringBuilder(BuildTextPart(values, preserveUnresolvedPlaceholderNames));
            foreach(var part in _subParts)
            {
                result.Append(part.BuildText(values, preserveUnresolvedPlaceholderNames));
            }
            return result.ToString();
        }

        protected abstract string BuildTextPart(IReadOnlyDictionary<string, string> values,
             bool preserveUnresolvedPlaceholderNames);
        
    }
}