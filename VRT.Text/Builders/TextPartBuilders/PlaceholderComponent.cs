using System.Collections.Generic;

namespace VRT.Text.TextBuilders
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class PlaceholderComponent: TextComponent
    {        
        public PlaceholderComponent(string placeholder)
        {
            Placeholder = placeholder;
        }
        /// <summary>
        /// Placeholder name
        /// </summary>
        public string Placeholder { get; }
        public override string ToString() => Placeholder;

        /// <summary>
        /// Flaga określająca czy nalezy wyświetlić nazwę etykiety jeśli w metadanych nie ma  wartości
        /// </summary>
        protected override string BuildTextPart(IReadOnlyDictionary<string, string> values,
            bool preserveUnresolvedPlaceholderNames)
        {
            if (null != values && values.TryGetValue(Placeholder, out var value))
                return value;
            return preserveUnresolvedPlaceholderNames
                ? $"{{{Placeholder}}}"
                : null;           
        }        
    }
}
