using System;
using System.Collections.Generic;

namespace Scraper.Framework
{
    public class UriTracker : IUriTracker
    {
        protected HashSet<string> TrackedUris { get; }

        public UriTracker (bool ignoreCase = true)
        {
            TrackedUris = ignoreCase ? new HashSet<string>(StringComparer.OrdinalIgnoreCase) : new HashSet<string>();
        }
        public Uri TrackUri(Uri uri)
        {
            if (uri == null)
            {
                return null;
            }
            // ensure tracking is mutually exclusive
            lock (this)
            {
                if (!TrackedUris.Add(uri.ToString()))
                {
                    return null;
                }
            }
            return uri;
        }

        public Uri TrackUri(string url)
        {
            return string.IsNullOrEmpty(url) ? null : TrackUri(new Uri(url));
        }
    }
}
