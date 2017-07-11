using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace SteveHemond.MusicSheetViewer.ViewModels.Partitions
{
    public class CommandBarViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand AddToPlaylistCommand { get; set; }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext) { }
    }
}