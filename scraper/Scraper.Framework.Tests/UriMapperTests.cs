using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scraper.Framework.Tests
{
    [TestClass]
    public class UriMapperTests
    {
        [TestMethod]
        public void Test ()
        {
            var m = new UriMapper("http://example.com");

            Assert.IsNull(m.MapLink(null), "MapLink(null) => null");
            Assert.IsNull(m.MapLink("http://another-example.com/abc"), "MapLink(<another domain>) => null");

            Assert.AreEqual("http://example.com/abc", m.MapLink("http://example.com/abc").ToString(), "MapLink(<absolute>) => absolute");
            Assert.AreEqual("http://example.com/a/b/c", m.MapLink("/a/b/c").ToString(), "MapLink(<relative>) => absolute");
            Assert.AreEqual("http://example.com/a/b/c", m.MapLink("a/b/c").ToString(), "MapLink(<relative>) => absolute");


            Assert.AreEqual("http://example.com/abc", m.MapLink("/abc?query").ToString(), "MapLink(<with query>) => without query");

            Assert.AreEqual("http://example.com/abc", m.MapLink("/abc#cool-stuff").ToString(), "MapLink(<with hash>) => without hash");
        }
        [TestMethod]
        public void MapLinkStaysInDomain()
        {
            var m = new UriMapper("http://example.com");
            Assert.IsNull(m.MapLink("http://another-example.com/abc"));
        }
    }
}
