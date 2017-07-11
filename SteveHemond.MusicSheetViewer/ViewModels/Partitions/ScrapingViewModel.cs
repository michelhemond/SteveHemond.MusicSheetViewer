using Prism.Mvvm;
using Prism.Regions;
using SteveHemond.MusicSheetViewer.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SteveHemond.MusicSheetViewer.ViewModels.Partitions
{
    class ScrapingViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private readonly ScrapingService scrapingService;

        private bool isScrapingStarted;
        public bool IsScrapingStarted
        {
            get => isScrapingStarted;
            set => SetProperty(ref isScrapingStarted, value);
        }

        private string currentFile;
        public string CurrentFile
        {
            get => currentFile;
            set => SetProperty(ref currentFile, value);
        }

        private int currentFileIndex;
        public int CurrentFileIndex
        {
            get => currentFileIndex;
            set => SetProperty(ref currentFileIndex, value);
        }

        private int fileCount;
        public int FileCount
        {
            get => fileCount;
            set => SetProperty(ref fileCount, value);
        }

        public string ProgressionText => CurrentFileIndex + " / " + FileCount;

        public ScrapingViewModel(IRegionManager regionManager, ScrapingService scrapingService)
        {
            this.regionManager = regionManager;
            this.scrapingService = scrapingService;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            CurrentFileIndex = 0;
            await Task.Run(Scrape);
            regionManager.RequestNavigate("ContentRegion", "PartitionsView");
            regionManager.RequestNavigate("CommandBarRegion", "Partitions.CommandBarView");
        }

        public async Task Scrape()
        {
            var filesToScrape = scrapingService.GetFilesToScrape();

            FileCount = filesToScrape.Count();

            foreach (var fileToScrape in filesToScrape)
            {
                CurrentFileIndex++;
                CurrentFile = fileToScrape.Name;
                RaisePropertyChanged(nameof(ProgressionText));
                IsScrapingStarted = true;
                await scrapingService.ScrapeFileAsync(fileToScrape);
            }
        }
    }
}