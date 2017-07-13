using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace SteveHemond.MusicSheetViewer.ViewModels.Playlists
{
    public class CommandBarViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand AddPartitionsToPlaylistCommand { get; set; }

        public DelegateCommand RemovePartitionsFromPlaylistCommand { get; set; }

        public DelegateCommand DeletePlaylistCommand { get; set; }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext) { }
    }
}