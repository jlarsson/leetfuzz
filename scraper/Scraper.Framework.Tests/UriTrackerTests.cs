using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scraper.Framework.Tests
{
    [TestClass]
    public class UriTrackerTests
    {
        [TestMethod]
        public void Test() {
            var t = new UriTracker();

            Assert.IsNull(t.TrackUri((Uri)null), "TrackUri(null) => null");

            Assert.AreEqual("http://example.com/abc", t.TrackUri("http://example.com/abc").ToString(), "TrackUri(<first occurence uri>) => uri");

            Assert.IsNull(t.TrackUri("http://example.com/abc"), "TrackUri(<second occurence uri>) => null");

            Assert.IsNull(t.TrackUri("http://example.com/ABC"), "TrackUri(<second occurence uri with different casing>) => null");
        }
    }
}
