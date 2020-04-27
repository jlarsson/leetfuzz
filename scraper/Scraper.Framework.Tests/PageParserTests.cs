using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Scraper.Framework.Tests
{
    [TestClass]
    public class PageParserTests
    {
        [TestMethod]
        public void Test() {
            var p = new PageParser();

            const string content = @"
                <html>
                    <body>
                        <a href=""/abc"" /> a self closed
                        <a href=""http://example.com/abc"">quite qwellformed</a>
                        <a href=""javascript:void(0)"">js...</a>
            ";

            CollectionAssert.AreEqual(
                new[] { 
                    "/abc",
                    "http://example.com/abc",
                    "javascript:void(0)"
                },
                p.ParseLinks(content).ToArray());
        }
    }
}
