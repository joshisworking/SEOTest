using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SEOTest.Utilities
{
    public class TextUtility
    {
        private static char[] delimiters = { ' ', '—', '\r' };
        private static string[] ignoreList = { "a", "an", "at", "and", "for", "in", "is", "of", "on", "the" };

        /// <summary>
        /// Sorts the list of key-value pairs by the value in descending order.
        /// </summary>
        /// <param name="dict">The list of key-value pairs to be sorted.</param>
        /// <returns>The sorted dictionary of key-value pairs.</returns>
        public static Dictionary<string, int> SortByValue(Dictionary<string, int> dict)
        {
            var sortedList = from entry in dict orderby entry.Value descending select entry;
            return sortedList.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        /// <summary>
        /// Trims non-alphanumeric characters from the beginning and end of the input string using a regular expression pattern.
        /// </summary>
        /// <param name="str">The input string to be trimmed.</param>
        /// <returns>The trimmed string with non-alphanumeric characters removed and converted to lowercase.</returns>
        private static string TrimNonAlphanumericReg(string str)
        {
            string pattern = "^[^a-zA-Z0-9]+|[^a-zA-Z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.Replace(str, "").ToLower();
        }

   
        /// <summary>
        /// Convert input string to a List of words and number of occurrences in string
        /// </summary>
        /// <param name="content">Input string to be analyzed</param>
        /// <param name="startValue">Value to assign as new words are added to List</param>
        /// <returns>List of words and number of occurrences</returns>
        public static Dictionary<string, int> AnalyzeText(string content, int startValue)
        {
            
            Array array = content.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> map = new Dictionary<string, int>();

            // Sanitize word
            foreach (string word in array)
            {
                string trimmedWord = TrimNonAlphanumericReg(word);

                // Do nothing if word is a stop word
                if (ignoreList.Contains(trimmedWord) || String.IsNullOrEmpty(trimmedWord))
                {
                    continue;
                }

                // Increment word count or add word to map if not already present
                if (map.ContainsKey(trimmedWord))
                {
                    map[trimmedWord] += 1;
                }
                else
                {
                    map.Add(trimmedWord, startValue);
                }
            }

            return map;
        }

        /// <summary>
        /// Updates the values in the input dictionary based on the corresponding values from the comparison dictionary.
        /// </summary>
        /// <param name="inputList">The dictionary to update.</param>
        /// <param name="comparisonList">The dictionary containing the values to update from.</param>
        public static void UpdateCountOfExistingListValues(
            Dictionary<string, int> inputList,
            Dictionary<string, int> comparisonList)
        {

            var commonKeys = inputList.Keys.Intersect(comparisonList.Keys);

            foreach (string key in commonKeys)
            {
                inputList[key] = comparisonList[key];
            }
        }
    }
}

