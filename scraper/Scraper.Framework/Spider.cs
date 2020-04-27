using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public class Spider: ISpider
    {
        public Spider(UriMapper uriMapper, UriTracker uriTracker, PageArchive pageArchive, PageParser pageParser, ILogger logger)
        {
            UriMapper = uriMapper;
            UriTracker = uriTracker;
            PageArchive = pageArchive;
            PageParser = pageParser;
            Logger = logger;
        }

        public IUriMapper UriMapper { get; }
        public IUriTracker UriTracker { get; }
        public IPageArchive PageArchive { get; }
        public IPageParser PageParser { get; }
        public ILogger Logger { get; }

        public Task ProcessPage(string link)
        {
            return ProcessPage(UriMapper.MapLink(link));
        }
        public async Task ProcessPage(Uri uri)
        {
            var pageUri = UriTracker.TrackUri(UriMapper.MapUri(uri));
            if (pageUri == null)
            {
                Logger.LogDebug($"skipped uri: {uri}");
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
