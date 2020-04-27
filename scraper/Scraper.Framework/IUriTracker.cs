using System;

namespace Scraper.Framework
{
    /// <summary>
    /// IUriTracker: registry of visited urls
    /// </summary>
    public interface IUriTracker
    {
        Uri TrackUri(Uri uri);
    }
}
