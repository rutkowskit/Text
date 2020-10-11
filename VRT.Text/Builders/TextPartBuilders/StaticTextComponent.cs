using System.Collections.Generic;

namespace VRT.Text.TextBuilders
{
    /// <inheritdoc />
    /// <summary>
    /// Visitor dokładający tekst stały do obiektu wizytującego
    /// </summary>
    internal sealed class StaticTextComponent : TextComponent
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="staticText">Tekst stały jaki visitor będzie dokładał</param>
        internal StaticTextComponent(string staticText)
        {
            StaticText = staticText;
        }
        /// <summary>
        /// Tekst stały jaki visitor będzie dokładał
        /// </summary>
        public string StaticText { get; }

        protected override string BuildTextPart(IReadOnlyDictionary<string, string> values,
            bool preserveUnresolvedPlaceholderNames) => StaticText;                            
    }
}
