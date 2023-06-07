using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SEOTest
{
	public class Utility
    {
        /// <summary>
        /// Parses the HTML content and extracts the plain text without any HTML markup.
        /// </summary>
        /// <param name="content">The HTML content to parse.</param>
        /// <returns>The plain text content without any HTML markup.</returns>
        public static string ParseHtml(string content)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            string textContent = htmlDoc.DocumentNode.InnerText.Trim();
            return textContent;
        }

        /// <summary>
        /// Sorts the list of key-value pairs by the value in descending order.
        /// </summary>
        /// <param name="list">The list of key-value pairs to be sorted.</param>
        /// <returns>The sorted list of key-value pairs.</returns>
        public static List<KeyValuePair<string, int>> SortByValue(List<KeyValuePair<string, int>> list)
        {
            list.Sort((x, y) => y.Value.CompareTo(x.Value));
            return list;
        }

        /// <summary>
        /// Trims non-alphanumeric characters from the beginning and end of the input string using a regular expression pattern.
        /// </summary>
        /// <param name="str">The input string to be trimmed.</param>
        /// <returns>The trimmed string with non-alphanumeric characters removed and converted to lowercase.</returns>
        public static string TrimNonAlphanumericReg(string str)
        {
            string pattern = "^[^a-zA-Z0-9]+|[^a-zA-Z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.Replace(str, "").ToLower();
        }

        /// <summary>
        /// Retrieves the content of a website specified by the given URL.
        /// </summary>
        /// <param name="url">The URL of the website to fetch content from.</param>
        /// <returns>The content of the website as a string.</returns>
        public static async Task<string> GetWebsiteContent(string url)
        {
            try
            {
                // Create a new instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Send a GET request to the specified URL
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful and return string accordingly
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return "Fetch failed";
                    }
                }
            }
            catch (Exception ex)
            {
                return "An error occurred. Please try later.";
            }
        }

        /// <summary>
        /// Convert input string to a List of words and number of occurrences in string
        /// </summary>
        /// <param name="content">input string to be analyzed</param>
        /// <returns>List of words and number of occurrences</returns>
        public static List<KeyValuePair<string, int>> AnalyzeText(string content)
        {
            char[] delimiters = { ' ', '—', '\r' };
            Array array = content.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> map = new Dictionary<string, int>();
            string[] ignoreList = { "a", "an", "at", "and", "for", "in", "is", "of", "on", "the" };

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
                    map.Add(trimmedWord, 1);
                }
            }

            return map.ToList();
        }
    }
}

