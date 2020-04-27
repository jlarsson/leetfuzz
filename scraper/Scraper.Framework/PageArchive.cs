using System;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public class PageArchive : IPageArchive
    {
        public Task SavePage(Uri uri, string content)
        {
            Console.WriteLine($"Saving ${uri}");
            return Task.CompletedTask;
        }
    }
}
