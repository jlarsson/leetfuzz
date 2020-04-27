using System;
using System.Collections.Generic;

namespace Scraper.Framework
{
    public class UriTracker : IUriTracker
    {
        protected Func<Uri, string> UriKeyFactory;

        public static readonly Func<Uri, string> DefaultUriKeyFactory = (Uri uri) => uri.ToString().ToLower();

        protected HashSet<string> TrackedUris { get; } = new HashSet<string>();


        public UriTracker(): this(DefaultUriKeyFactory) { }

        public UriTracker(Func<Uri, string> uriKeyFactory)
        {
            UriKeyFactory = uriKeyFactory ?? DefaultUriKeyFactory;
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
                if (!TrackedUris.Add(UriKeyFactory(uri)))
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
