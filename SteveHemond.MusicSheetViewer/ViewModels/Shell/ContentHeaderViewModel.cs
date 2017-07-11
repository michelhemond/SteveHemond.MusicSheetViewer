using MahApps.Metro.IconPacks;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace SteveHemond.MusicSheetViewer.ViewModels.Shell
{
    public class ContentHeaderViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private string displayName;
        public string DisplayName
        {
            get => displayName;
            set => SetProperty(ref displayName, value);
        }

        private PackIconMaterialKind icon;
        public PackIconMaterialKind Icon
        {
            get => icon;
            set => SetProperty(ref icon, value);
        }

        public ContentHeaderViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            DisplayName = navigationContext.Parameters["DisplayName"] as string;
            Icon = (PackIconMaterialKind)navigationContext.Parameters["Icon"];
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}