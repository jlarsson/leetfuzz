using System;

namespace Scraper.Framework
{
    public class UriMapper : IUriMapper
    {
        public UriMapper(Uri baseUri) {
            BaseUri = baseUri;
        }

        public UriMapper(string baseUri): this(new Uri(baseUri))
        {
        }

        public Uri BaseUri { get; }

        public Uri MapLink(string link)
        {
            return string.IsNullOrEmpty(link) ? null : ValidateAndSanitizeUri(new Uri(BaseUri, link));
        }

        public Uri MapUri(Uri uri)
        {
            return uri == null ? null : ValidateAndSanitizeUri(new Uri(BaseUri,uri));
        }

        protected Uri ValidateAndSanitizeUri(Uri uri) { 
            if (uri == null)
            {
                return null;
            }
            if (!uri.IsAbsoluteUri)
            {
                return null;
            }

            if (BaseUri.Equals(uri))
            {
                return uri;
            }

            if (!BaseUri.IsBaseOf(uri))
            {
                return null;
            }

            return new UriBuilder(uri)
            {
                Query = null,
                Fragment = null,

            }.Uri;
        }
    }
}
