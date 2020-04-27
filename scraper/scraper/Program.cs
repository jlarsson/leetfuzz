using Microsoft.Extensions.Logging;
using Scraper.Framework;
using System;
using System.IO;

namespace scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create spider, rhun it and wait for completion
            // NOTE: we wire every dependency manually and skips DI frameworks altogehter, though teh code is somewhat prepared for it
            CreateSpider(
                url: "https://tretton37.com:443",
                archiveRoot: Path.Join(Directory.GetCurrentDirectory(), ".\\.archive"),
                logLevel: LogLevel.Information
                )
                .ProcessPage("/")
                .Wait();
        }

        static ISpider CreateSpider(string url, string archiveRoot, LogLevel logLevel)
        {
            var logger = CreateLogger(logLevel);
            Uri root = new Uri(url);
            return new Spider(
                uriMapper: new UriMapper(root),
                uriTracker: new UriTracker(),
                pageArchive: new PageArchive(
                    options: new PageArchiveOptions { ArchiveRoot = archiveRoot },
                    logger: logger),
                pageParser: new PageParser(),
                logger: logger
            );
        }

        static ILogger CreateLogger(LogLevel logLevel)
        {
            return new ConsoleLogger(logLevel);
        }
    }
}
