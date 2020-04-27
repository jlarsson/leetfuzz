using System;

namespace Scraper.Framework
{
    public interface IUriMapper
    {
        Uri MapLink(string link);
        Uri MapUri(Uri uri);
    }
}
