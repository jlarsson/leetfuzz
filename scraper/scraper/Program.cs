using Microsoft.Extensions.Logging;
using Scraper.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Scrape(
                url: "https://tretton37.com:443",
                archiveRoot: Path.Join(Directory.GetCurrentDirectory(), ".\\.archive")
                ).Wait();
        }

        static async Task Scrape(string url, string archiveRoot)
        {
            Uri root = new Uri(url);
            var spider = new Spider()
            {
                UriMapper = new UriMapper(root),
                UriTracker = new UriTracker(),
                PageArchive = new PageArchive(archiveRoot),
                PageParser = new PageParser()
            };
            await spider.ProcessPage("/");
        }
    }
}
