using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scraper.Framework
{
    public class PageArchive : IPageArchive
    {
        public PageArchive(string archiveRoot)
        {
            ArchiveRoot = archiveRoot;
        }

        public string ArchiveRoot { get; }

        public async Task SavePage(Uri uri, string content)
        {
            var folderPath = Path.GetFullPath(Path.Combine(
                new[] { ArchiveRoot }.Concat(uri.LocalPath.Split('/')).ToArray()));
            var filePath = Path.Combine(folderPath, ".contents");
            try
            {
                // NOTE: Folder creating will be a race hotspot for sure...
                await Task.Run(() => Directory.CreateDirectory(folderPath));

                await Task.Run(() => File.WriteAllText(filePath, content));

                Console.WriteLine($"saved {uri} to {filePath}");
            }
            catch (Exception err)
            {
                Console.Error.WriteLine($"failed to save {uri} to {folderPath}");
            }
        }
    }
}
