using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public class Spider: ISpider
    {
        public Spider(IUriMapper uriMapper, IUriTracker uriTracker, IPageLoader pageLoader, IPageArchive pageArchive, IPageParser pageParser, ILogger logger)
        {
            UriMapper = uriMapper;
            UriTracker = uriTracker;
            PageLoader = pageLoader;
            PageArchive = pageArchive;
            PageParser = pageParser;
            Logger = logger;
        }

        public IUriMapper UriMapper { get; }
        public IUriTracker UriTracker { get; }
        public IPageLoader PageLoader { get; }
        public IPageArchive PageArchive { get; }
        public IPageParser PageParser { get; }
        public ILogger Logger { get; }

        public Task ProcessPage(string link)
        {
            return ProcessPage(UriMapper.MapLink(link));
        }
        /// <summary>
        /// Process page by
        /// - skipping if already visited/processed
        /// - skipping if invalid (i.e. leaves main domain)
        /// - archive contents
        /// - recursively process links
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
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
            var pageContent = await PageLoader.DownloadPage(pageUri);

            // parallell: Ensure page is saved
            var saveCompletion = PageArchive.SavePage(pageUri, pageContent);
            // paralell: Process links
            var crawlCompletion = ProcessLinks(pageUri, pageContent);

            // wait for all parallell
            await Task.WhenAll(saveCompletion, crawlCompletion);
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
