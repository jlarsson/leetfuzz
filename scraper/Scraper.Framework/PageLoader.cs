using System;
using System.Net;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public class PageLoader : IPageLoader
    {
        public async Task<string> DownloadPage(Uri uri)
        {
            using (var webClient = new WebClient())
            {
                // TODO: here is a good place for a monitor to avoid DOS
                // NOTE: we actually want DOS in this particual case...
                return await webClient.DownloadStringTaskAsync(uri);
            }
        }
    }
}
