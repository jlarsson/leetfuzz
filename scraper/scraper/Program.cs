using Scraper.Framework;
using System;
using System.Threading.Tasks;

namespace scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Scrape("https://tretton37.com").Wait();

            Console.WriteLine("done");
        }

        static async Task Scrape(string url)
        {
            Uri root = new Uri(url);
            var spider = new Spider()
            await spider.ProcessPage("/");
        }
    }
}
