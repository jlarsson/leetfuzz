using System;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public interface IPageLoader
    {
        Task<string> DownloadPage(Uri uri);
    }
}
