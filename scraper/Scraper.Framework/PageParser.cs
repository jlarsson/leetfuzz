using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Scraper.Framework
{
    public class PageParser : IPageParser
    {
        public IEnumerable<string> ParseLinks(string content)
        {
            // regular expression taken from
            // - https://stackoverflow.com/questions/15926142/regular-expression-for-finding-href-value-of-a-a-link
            // - https://regex101.com/r/rMAHrE/1

            var matches = new Regex("<a\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1").Matches(content ?? "")
                .Cast<Match>()
                .Select(m => m.Groups[2].Value)
                .ToArray();


            return matches;
        }
    }
}
