using System;
using SEOTest.Utilities;

namespace SEOTest.Models
{
	public class AnalysisResults
	{
		private Dictionary<string, int> _wordList;

		private Dictionary<string, int> _metaList;

        public Dictionary<string, int> WordList
		{
			get { return _wordList; }
			set
			{
                // If Value has no content, assign message. Else, assign value.
                if (value.Count == 0)
				{

                    _wordList = (new Dictionary<string, int>()
					{
						{"No content provided", 0 }
					});

                }
				else
				{
                    value = TextUtility.SortByValue(value);
                    _wordList = value;

				}
			}
		}
		public Dictionary<string, int> MetaList
        {
            get { return _metaList; }
            set
            {
                // If Value has no content, assign message. Else, assign value.
                if (value.Count == 0)
                {

                    _metaList = (new Dictionary<string, int>()
                    {
                        {"No meta tags found in submission or not a valid HTML document", 0 }
                    });

                }
                else
                {
                    value = TextUtility.SortByValue(value);
                    _metaList = value;
                }
            }
        }
        public int LinkCount { get; set; }

        public AnalysisResults(Dictionary<string, int> wordList, Dictionary<string, int> metaList, int linkCount)
        {
            WordList = wordList;
            MetaList = metaList;
            LinkCount = linkCount;
        }
	}
}

