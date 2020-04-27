using System;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public interface IPageArchive
    {
        /// <summary>
        /// Persist crawled page
        /// </summary>

        Task SavePage(Uri uri, string content);
    }
}
