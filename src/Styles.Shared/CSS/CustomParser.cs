using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Styles
{
	public abstract class CustomCssParser<T> where T : CssParameters
	{
		protected Dictionary<string, PropertyInfo> customProperties;

		public CustomCssParser()
		{
			// Dictionary of all properties found on TextStyleParameters
			customProperties = typeof(T).GetRuntimeProperties()
				.Select(p => new { p, attr = p.GetCustomAttributes(typeof(CssAttribute), true) })
				.Where(prop => prop.attr.Count() == 1)
				.Select(obj => new { Property = obj.p, Attribute = obj.attr.First() as CssAttribute })
				.ToDictionary(t => t.Attribute.Name, t => t.Property);
		}

		public Dictionary<string, T> Parse(CssRuleSet ruleSet)
		{
			var parsedStyles = new Dictionary<string, T>();

			// Process all the rules
			foreach (var rule in ruleSet.Rules)
			{
				// Process each selector
				foreach (var selector in rule.Selectors)
				{
					// If it doesnt exist, create it
					if (!parsedStyles.ContainsKey(selector))
						parsedStyles[selector] = (T)Activator.CreateInstance(typeof(T), selector);

					var curStyle = parsedStyles[selector];
					ParseCSSRule(ref curStyle, rule, ruleSet.Variables);
				}
			}

			return parsedStyles;
		}

		public abstract T MergeRule(T curStyle, string css, bool clone);

		public abstract void ParseCSSRule(ref T curStyle, CssParserRule rule, Dictionary<string, string> cssVariables);

		public abstract string ParseToCSSString(string tagName, T style);
	}
}

