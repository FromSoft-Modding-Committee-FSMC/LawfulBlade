using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeManager.Type
{
    public static class StringArrayExtensions
    {
        /// <summary>
        /// Checks to see if the given item is inside the array. 
        /// <br/>Warning: SLOW.
        /// </summary>
        /// <param name="array">The array to check</param>
        /// <param name="value">The value to check for</param>
        /// <returns>True if the item exists, false otherwise</returns>
        public static bool Contains(this string[] array, string value)
        {
            foreach(string item in array)
                if(item == value) return true;

            return false;
        }

        /// <summary>
        /// Checks to see if any of the given items are inside the array. 
        /// <br/>Warning: VERY FUCKING SLOW.
        /// </summary>
        /// <param name="arrayA">The array to check</param>
        /// <param name="arrayB">The values to check for</param>
        /// <returns>True if the item exists, false otherwise</returns>
        public static bool Contains(this string[] arrayA, string[] arrayB)
        {
            foreach (string itemA in arrayA)
                foreach (string itemB in arrayB)
                    if (itemA == itemB)
                        return true;

            return false;
        }
    }
}
