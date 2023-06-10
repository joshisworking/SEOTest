using System;
using System.Text;
using HtmlAgilityPack;

namespace SEOTest.Utilities
{
	public class HtmlUtility
	{

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
                Console.Write(ex);
                return "An error occurred. Please try later.";
            }
        }


        /// <summary>
        /// Extracts the content attribute of meta tags and returns a string containing all entries.
        /// </summary>
        /// <param name="htmlDocument">The HtmlDocument to be analyzed.</param>
        /// <returns>A string containing words extracted from the content attribute of meta tags.</returns>
        public static string ExtractMetaTagContent(HtmlDocument htmlDocument)
        {
            StringBuilder sb = new StringBuilder();

            // Select all meta tags from the HTML document
            IEnumerable<HtmlNode> metaTags = htmlDocument.DocumentNode.SelectNodes("//meta");

            // If no meta tags found, return empty string
            if (metaTags == null) { return sb.ToString(); }

            // Extract the content attribute value, convert it to lowercase, and add to StringBuilder
            foreach (HtmlNode metaTag in metaTags)
            {
                sb.Append(' ');
                sb.Append(metaTag.GetAttributeValue("content", "").ToLower());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Counts the number of external links in an HtmlDocument. Assumes any absolute path is external.
        /// </summary>
        /// <param name="htmlDocument">The HtmlDocument to analyze.</param>
        /// <returns>The count of external links found in the HtmlDocument.</returns>
        public static int CountExternalLinks(HtmlDocument htmlDocument)
        {
            int linkCount = 0;

            foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//a"))
            {
                if (Uri.IsWellFormedUriString(link.Attributes["href"].Value, UriKind.Absolute))
                {
                    linkCount++;
                }
            }

            return linkCount;
        }

    }
}

