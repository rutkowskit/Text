using System.Collections.Concurrent;
using System.Collections.Generic;
using VRT.Text.TextBuilders;

namespace VRT.Text.Builders
{
    /// <summary>
    /// Templated string builder
    /// </summary>
    public sealed class TemplateStringBuilder
    {
        private static readonly ConcurrentDictionary<string, TemplateStringBuilder> Cache = new ConcurrentDictionary<string, TemplateStringBuilder>();

        private readonly string _template;
        private readonly TextComponent _textBuilder;

        private TemplateStringBuilder(string template, TextComponent composite)
        {
            _template = template;
            _textBuilder = composite;
        }

        /// <summary>
        /// Provides empty TemplateStringBuilder
        /// </summary>
        public static TemplateStringBuilder Empty => new TemplateStringBuilder(null, TextComponent.Empty);
        
        /// <summary>
        /// Creates instance of TemplateStringBuilder for given template
        /// </summary>
        /// <param name="template">template text</param>
        /// <returns>TemplateStringBuilder instance</returns>
        public static TemplateStringBuilder Create(string template)
        {
            return null == template
                ? Empty
                : Cache.GetOrAdd(template, (key) =>
                {
                    var builder = template.AsTextComposite();
                    return new TemplateStringBuilder(key, builder);
                });
        }      
        /// <summary>
        /// Overriden default ToString method
        /// </summary>
        /// <returns>Retunrs originla template text</returns>
        public override string ToString()
        {            
            return _template;
        }
        /// <summary>
        /// Explicit cast operator from string to TemplateStringBuilder
        /// </summary>
        /// <param name="template">Template text</param>
        public static explicit operator TemplateStringBuilder(string template) => Create(template);
        /// <summary>
        /// Explicit cast operator from TemplateStringBuilder to string
        /// </summary>
        /// <param name="templateString">Template text</param>
        public static explicit operator string(TemplateStringBuilder templateString) => templateString?.ToString();

        /// <summary>
        /// Creates string by replacing placeholders within template with provided placeholderValues
        /// </summary>
        /// <param name="placeholderValues">Placeholder values dictionary</param>        
        public string ToString(IReadOnlyDictionary<string, string> placeholderValues)
            => ToString(placeholderValues, false);

        /// <summary>
        /// Creates string by replacing placeholders within template with provided placeholderValues
        /// </summary>
        /// <param name="placeholderValues">Placeholder values dictionary</param>
        /// <param name="preserveUnresolvedPlaceholderNames">Preserves placeholders in output string if no corresponding value was provided</param>        
        public string ToString(IReadOnlyDictionary<string, string> placeholderValues, 
            bool preserveUnresolvedPlaceholderNames)
        {            
            return _textBuilder.BuildText(placeholderValues, preserveUnresolvedPlaceholderNames);            
        }
    }
}
