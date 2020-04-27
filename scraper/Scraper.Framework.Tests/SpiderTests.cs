using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper.Framework.Tests
{
    [TestClass]
    public partial class SpiderTests
    {
        [TestMethod]
        public void EachPageIsVisitedExactlyOnce()
        {
            // Fake site strucure
            var siteStructure = new Dictionary<string, string>()
            {
                {"http://example.com/", @"<html><body><a href=""/foo"">foo</a><a href=""http://example.com/foo"">second foo</a></body></html>" },
                {"http://example.com/foo", @"<html><body><a href=""http://example.com/foo"">absolute foo</a><a href=""/bar"">relative bar</a></body></html>" },
                {"http://example.com/bar", @"" },
                {"http://example.com/never-linked-to", @"" }
            };


            // Keep track of loaded pages and the sequence of loads
            var loadedPages = new List<string>();

            // Keep track of saved pages and the sequence of saves
            var savedPages = new List<string>();

            // Load page with assertions on validity
            Task<string> AssertiveLoadPage(Uri uri)
            {
                var key = uri.ToString();
                Assert.IsTrue(siteStructure.ContainsKey(key), $"Invalid page download of {uri}");

                loadedPages.Add(key);
                return Task.FromResult(siteStructure[key]);
            }

            // Save page with assertions on validity
            Task AssertiveSavePage(Uri uri, string content)
            {
                var key = uri.ToString();
                Assert.IsTrue(siteStructure.ContainsKey(key), $"Invalid page download of {uri}");
                savedPages.Add(key);
                return Task.CompletedTask;
            }

            // Do a fake crawl
            const string baseUri = "http://example.com";
            var spider = new Spider(
                uriMapper: new UriMapper(baseUri),
                uriTracker: new UriTracker(uri => uri.ToString().ToLower()),
                pageParser: new PageParser(),
                pageLoader: new FakePageLoader(AssertiveLoadPage),
                pageArchive: new FakePageArchive(AssertiveSavePage),
                logger: NullLogger.Instance
                );

            spider.ProcessPage("/").Wait();


            // Check saves and loads
            // The sorted lists should each have exacly one occurence of accessibe pages in the graph
            loadedPages.Sort();
            savedPages.Sort();
            CollectionAssert.AreEqual(new[]
            {
                "http://example.com/",
                "http://example.com/bar",
                "http://example.com/foo"
            }, loadedPages);
            CollectionAssert.AreEqual(new[]
            {
                "http://example.com/",
                "http://example.com/bar",
                "http://example.com/foo"
            }, savedPages);
        }
    }
}
