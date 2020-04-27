using System;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public interface ISpider
    {
        Task ProcessPage(string link);
        Task ProcessPage(Uri uri);
    }
}
