using Spire.Pdf;
using SteveHemond.MusicSheetViewer.Data;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using SteveHemond.MusicSheetViewer.Helpers;

namespace SteveHemond.MusicSheetViewer.Services
{
    public class ScrapingService
    {
        private readonly SettingService settingsService;

        private readonly PartitionService partitionService;

        public ScrapingService(SettingService settingsService, PartitionService partitionService)
        {
            this.settingsService = settingsService;
            this.partitionService = partitionService;
        }

        public IEnumerable<FileInfo> GetFilesToScrape()
        {
            var path = settingsService.GetPdfFilePath();

            foreach(var filePath in Directory.GetFiles(path, "*pdf"))
            {
                var fileInfo = new FileInfo(filePath);

                if (!partitionService.PartitionExists(fileInfo.Name))
                {
                    yield return fileInfo;
                }
            }
        }

        public Task ScrapeFileAsync(FileInfo fileInfo)
        {
            return Task.Run(() =>
            {
                var partition = new Partition();

                var pages = GetPages(fileInfo.FullName).ToList();
                partition.Pages = pages;
                partition.FileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                partition.PageCount = pages.Count();
                partition.Thumbnail = pages.First().Image;

                partitionService.AddPartition(partition);
            });
        }

        private IEnumerable<Page> GetPages(string filePath)
        {
            PdfDocument pdfDocument = new PdfDocument();
            pdfDocument.LoadFromFile(filePath);

            for (var i = 0; i < pdfDocument.Pages.Count; i++)
            {
                var page = new Page()
                {
                    PageNumber = i + 1
                };

                var image = pdfDocument.SaveAsImage(i);

                using (MemoryStream memory = new MemoryStream())
                {
                    image.Save(memory, ImageFormat.Bmp);
                    memory.Position = 0;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    page.Image = bitmapImage.ToByteArray();
                }

                yield return page;
            }
        }
    }
}