using System;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public interface IPageArchive
    {
        Task SavePage(Uri uri, string content);
    }
}
