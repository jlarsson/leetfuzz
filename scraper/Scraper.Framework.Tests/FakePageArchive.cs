using System;
using System.Threading.Tasks;

namespace Scraper.Framework.Tests
{
    public partial class SpiderTests
    {
        internal class FakePageArchive : IPageArchive
        {

            public FakePageArchive(Func<Uri, string, Task> saver)
            {
                Saver = saver;
            }

            public Func<Uri, string, Task> Saver { get; }

            public Task SavePage(Uri uri, string content)
            {
                return Saver(uri, content);
            }
        }

    }
}
