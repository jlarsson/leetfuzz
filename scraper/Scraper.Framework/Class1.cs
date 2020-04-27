using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public interface IUriMapper
    {
        Uri MapLink(string link);
        Uri MapUri(Uri uri);
    }
    public interface IUriTracker
    {
        Uri TrackUri(Uri uri);
    }

    public interface IPageArchive
    {
        Task SavePage(Uri uri, string content);
    }

    public interface IPageParser
    {
        IEnumerable<string> ParseLinks(string content);
    }
    public class Spider
    {
        WebClient WebClient { get; set; } = new WebClient();
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
            var pageContent = await WebClient.DownloadStringTaskAsync(pageUri);

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
