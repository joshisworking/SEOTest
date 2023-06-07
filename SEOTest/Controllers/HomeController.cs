using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SEOTest.Models;
using static SEOTest.Utility;

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
        
        string sample;

        if (formData.InputOption == "text")
        {
            // If the input option is "text", assign the value of SampleText
            sample = formData.SampleText;
        }
        else
        {
            // If the input option is not "text", retrieve and assign the website content
            string content = await GetWebsiteContent(formData.SampleURL);
            sample = ParseHtml(content);
        }

        // Initialize word list
        List<KeyValuePair<string, int>> WordList;

        if (string.IsNullOrEmpty(sample)) {
            // If no input, return message
            WordList = (new Dictionary<string, int>()
            {
                {"No content provided", 0 }
            }).ToList();
        }
        else if (sample == "Fetch failed")
        {
            // If accessing site failed, return error page
            return View("UrlError");
        }
        else
        {
            // Else, analyze returned content
            WordList = AnalyzeText(sample);
        }

        return View("Results", SortByValue(WordList));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    



}
