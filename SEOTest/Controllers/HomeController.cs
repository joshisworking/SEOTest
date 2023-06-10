using System.Diagnostics;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using SEOTest.Models;
using SEOTest.Utilities;

namespace SEOTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(FormData formData)
    {
        HtmlDocument doc = new HtmlDocument();
        string sample = "";
        bool isHtmlDocument = true;

        if (formData.InputOption == "text")
        {
            // If the input option is "text", assign the value of SampleText
            sample = formData.SampleText;

            // Naive check if submission is a valie HTML document
            isHtmlDocument = sample.TrimStart().StartsWith("<!DOCTYPE html", StringComparison.OrdinalIgnoreCase);

            // If sample is valid HTML, load data and refresh sample with inner HTML text
            if (isHtmlDocument)
            {
                doc.LoadHtml(sample);
                sample = doc.DocumentNode.InnerText.Trim();
            }

        }
        else
        {
            // If the input option is not "text", retrieve and assign the website content
            string content = await HtmlUtility.GetWebsiteContent(formData.SampleURL);
            doc.LoadHtml(content);
            sample = doc.DocumentNode.InnerText.Trim();
        }

        Dictionary<string, int> wordList;
        Dictionary<string, int> metaTagWordList = new();
        int externalLinkCount = 0;

        if (sample == "Fetch failed")
        {
            // If accessing site failed, return error page
            return View("UrlError");
        }
        else if (isHtmlDocument)
        {
            // If submission is HTML document, obtain list of meta tag content, link count, and analyze content
            string metaTagWords = HtmlUtility.ExtractMetaTagContent(doc);
            metaTagWordList = TextUtility.AnalyzeText(metaTagWords, 0);
            externalLinkCount = HtmlUtility.CountExternalLinks(doc);
            wordList = TextUtility.AnalyzeText(sample, 1);

            // Updated count of MetaTagWords found in WordList
            TextUtility.UpdateCountOfExistingListValues(metaTagWordList, wordList);
        }
        else
        {
            // Else, analyze returned content only
            wordList = TextUtility.AnalyzeText(sample, 1);
        }

        AnalysisResults results = new AnalysisResults(wordList, metaTagWordList, externalLinkCount);

        return View("Results", results);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    



}
