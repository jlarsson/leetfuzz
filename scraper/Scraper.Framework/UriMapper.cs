using System;

namespace Scraper.Framework
{
    public class UriMapperOptions
    {
        public Uri BaseUri { get; set; }
    }
    public class UriMapper : IUriMapper
    {
        public UriMapper(UriMapperOptions options) {
            Options = options;
        }
        public UriMapper(Uri baseUri): this(new UriMapperOptions { BaseUri = baseUri }) {
        }

        public UriMapper(string baseUri): this(new Uri(baseUri))
        {
        }

        public UriMapperOptions Options { get; }

        public Uri MapLink(string link)
        {
            return string.IsNullOrEmpty(link) ? null : ValidateAndSanitizeUri(ExpandUri(new Uri(Options.BaseUri, link)));
        }

        public Uri MapUri(Uri uri)
        {
            return uri == null ? null : ValidateAndSanitizeUri(ExpandUri(new Uri(Options.BaseUri,uri)));
        }

        protected Uri ExpandUri (Uri uri)
        {
            return new UriBuilder(uri).Uri;
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

            if (Options.BaseUri.Equals(uri))
            {
                return uri;
            }

            if (!Options.BaseUri.IsBaseOf(uri))
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
