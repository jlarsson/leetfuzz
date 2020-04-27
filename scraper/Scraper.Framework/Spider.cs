using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public class Spider
    {
        public IUriMapper UriMapper { get; set; }
        public IUriTracker UriTracker { get; set; }
        public IPageArchive PageArchive { get; set; }
        public IPageParser PageParser { get; set; }

        public Task ProcessPage(string link)
        {
            return ProcessPage(UriMapper.MapLink(link));
        }
        public async Task ProcessPage(Uri uri)
        {
            var pageUri = UriTracker.TrackUri(UriMapper.MapUri(uri));
            if (pageUri == null)
            {
                // uri is invalid, already visited or in progress
                return;
            }

            // Fetch page

            // TODO: This is rude (DOS) and we should actually throttle this through a semaphore
            using (WebClient webClient = new WebClient())
            {
                var pageContent = await webClient.DownloadStringTaskAsync(pageUri);

                // parallell: Ensure page is saved
                var saveCompletion = PageArchive.SavePage(pageUri, pageContent);
                // paralell: Process links
                var crawlCompletion = ProcessLinks(pageUri, pageContent);

                // wait for all parallell
                await Task.WhenAll(saveCompletion, crawlCompletion);
            }
        }

        private async Task ProcessLinks(Uri uri, string content)
        {
            var processCompletions = PageParser.ParseLinks(content)
                .Select(UriMapper.MapLink)
                .Select(ProcessPage)
                .ToArray();

            await Task.WhenAll(processCompletions);
        }
    }
}
