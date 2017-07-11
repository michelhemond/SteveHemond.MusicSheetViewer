using Prism.Mvvm;
using Prism.Regions;
using SteveHemond.MusicSheetViewer.Services;

namespace SteveHemond.MusicSheetViewer.ViewModels.Settings
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly SettingService settingsService;

        private string pdfFilePath;
        public string PdfFilePath
        {
            get => pdfFilePath;
            set
            {
                SetProperty(ref pdfFilePath, value);
                settingsService.SetPdfFilePath(value);
            }
        }

        public bool IsPdfFilePathSet
        {
            get => !string.IsNullOrEmpty(PdfFilePath);
        }

        public SettingsViewModel(SettingService settingsService)
        {
            this.settingsService = settingsService;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            PdfFilePath = settingsService.GetPdfFilePath();
        }
    }
}