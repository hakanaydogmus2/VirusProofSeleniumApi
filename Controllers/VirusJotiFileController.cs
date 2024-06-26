﻿using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using VirusProofSelenium.Models;
using System.Threading;
using OpenQA.Selenium.Firefox;
using VirusProofSelenium.Helper;

namespace VirusProofSelenium.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VirusJotiFileController : Controller
    {
        private readonly PathCreator _pathCreator;
        public VirusJotiFileController(PathCreator pathCreator)
        {
            _pathCreator = pathCreator;
        }
        [HttpPost]
        public async Task<IActionResult> GetVirusJotiFilesResult(IFormFile file)
        {
            IWebDriver driver = new FirefoxDriver();

            // Web sayfasına gidin
            driver.Navigate().GoToUrl("https://virusscan.jotti.org/en-US/scan-file"); // URL'yi buraya ekleyin
            IWebElement webElement = driver.FindElement(By.Name("sample-file[]"));

            var path = await _pathCreator.getPathOfFile(file);
            Console.WriteLine(path);

            webElement.SendKeys(path);
            // "stack-horizontally results" sınıfına sahip öğeleri bulun
            Thread.Sleep(10000);
            var resultElements = driver.FindElements(By.CssSelector(".stack-horizontally.results"));

            List<string> titles = new List<string>();
            List<VirusJotiFile> virusJotiFiles = new List<VirusJotiFile>();

            foreach (var resultElement in resultElements)
            {
                
                var titleElements = resultElement.FindElements(By.CssSelector("[title]"));
                foreach (var titleElement in titleElements)
                {
                    
                    string title = titleElement.GetAttribute("title");
                    titles.Add(title);
                }
            }

            // Başlıkları yazdırın
            for (int titleIndex = 0; titleIndex < titles.Count - 1; titleIndex += 2)
            {
                Console.Write(titles[titleIndex]);
                var vjf = new VirusJotiFile();
                vjf.FileName = file.FileName;
                vjf.AntiVirus = titles[titleIndex];
                vjf.Result = titles[titleIndex + 1];
                virusJotiFiles.Add(vjf);
            }

            // Tarayıcıyı kapatın
            driver.Quit();
            return Ok(virusJotiFiles);
        }

    }
}
