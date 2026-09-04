using System.Collections.Generic;

namespace StacyClouds.C4Sharp.Core.Util
{
    
    /// <summary>
    /// Creates dictionaries from simple <c>name=value</c> string pairs.
    /// </summary>
    public class DictionaryUtils
    {
    
        /// <summary>
        /// Builds a dictionary from strings formatted as <c>name=value</c>.
        /// </summary>
        /// <param name="nameValuePairs">The pairs to parse.</param>
        /// <returns>A dictionary containing the parsed keys and values.</returns>
        /// <remarks>
        /// Entries without exactly one <c>=</c> separator are ignored.
        /// </remarks>
        public static Dictionary<string,string> Create(params string[] nameValuePairs)
        {
            Dictionary<string,string> map = new Dictionary<string, string>();

            if (nameValuePairs != null) {
                foreach (string nameValuePair in nameValuePairs)
                {
                    string[] tokens = nameValuePair.Split('=');
                    if (tokens.Length == 2)
                    {
                        map[tokens[0]] = tokens[1];
                    }
                }
            }

            return map;
        }
        
    }
    
}