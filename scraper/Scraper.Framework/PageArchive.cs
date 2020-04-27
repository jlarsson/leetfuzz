using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public class PageArchive : IPageArchive
    {
        public PageArchiveOptions Options { get; }
        public ILogger Logger { get; }

        public PageArchive(PageArchiveOptions options, ILogger logger)
        {
            Options = options;
            Logger = logger;
        }

        public async Task SavePage(Uri uri, string content)
        {
            // Outline
            //  Given http://somehting.com/a/b/c
            //  - ensure folder ...\a\b\c
            //  - write content to ...\a\b\c\.content

            var folderPath = Path.GetFullPath(Path.Combine(
                new[] { Options.ArchiveRoot }.Concat(uri.LocalPath.Split('/')).ToArray()));
            var filePath = Path.Combine(folderPath, ".contents");
            try
            {
                // NOTE: Folder creating will be a race hotspot for sure...
                await Task.Run(() => Directory.CreateDirectory(folderPath));

                await Task.Run(() => File.WriteAllText(filePath, content));

                Logger.LogInformation($"saved {uri} to {filePath}");
            }
            catch (Exception err)
            {
                Logger.LogError(err, $"failed to save {uri} to {folderPath}");
            }
        }
    }
}
