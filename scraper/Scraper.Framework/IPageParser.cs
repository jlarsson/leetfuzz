using System.Collections.Generic;

namespace Scraper.Framework
{
    public interface IPageParser
    {
        /// <summary>
        /// Return all links within given text
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        IEnumerable<string> ParseLinks(string content);
    }
}
