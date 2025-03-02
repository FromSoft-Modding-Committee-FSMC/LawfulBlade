using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
