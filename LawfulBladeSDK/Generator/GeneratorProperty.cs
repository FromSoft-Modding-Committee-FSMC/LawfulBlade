using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeSDK.Generator
{
    public class GeneratorProperty
    {
        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The property value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The property type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets the tool tip for the generator property
        /// </summary>
        public static string GetTooltip(object generator, GeneratorProperty property)
        {
            var fields = generator.GetType().GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (field.GetValue(generator) != property)
                    continue;

                return field.GetCustomAttribute<GeneratorPropertyTooltipAttribute>()?.Tooltip;
            }

            return string.Empty;
        }
    }
}
