using System;
using System.Threading.Tasks;

namespace Scraper.Framework.Tests
{
    public partial class SpiderTests
    {
        internal class FakePageLoader : IPageLoader
        {
            public FakePageLoader(Func<Uri, Task<string>> loader)
            {
                Loader = loader;
            }

            public Func<Uri, Task<string>> Loader { get; }

            public Task<string> DownloadPage(Uri uri)
            {
                return Loader(uri);
            }
        }
    }
}
