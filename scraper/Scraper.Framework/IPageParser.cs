using System.Collections.Generic;

namespace Scraper.Framework
{
    public class PageParser : IPageParser
    {
        public IEnumerable<string> ParseLinks(string content)
        {
            throw new System.NotImplementedException();
        }
    }
    public interface IPageParser
    {
        IEnumerable<string> ParseLinks(string content);
    }
}
