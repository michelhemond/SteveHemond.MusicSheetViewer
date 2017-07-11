using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SteveHemond.MusicSheetViewer.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SteveHemond.MusicSheetViewer.ViewModels.Playlists
{
    public class PlaylistsViewModel : BindableBase, INavigationAware
    {
        private readonly CommandBarViewModel commandBarViewModel;

        private readonly IRegionManager regionManager;

        private readonly PartitionService partitionService;

        private readonly PlaylistService playlistService;

        private ObservableCollection<PlaylistItemViewModel> playlists;
        public ObservableCollection<PlaylistItemViewModel> Playlists
        {
            get => playlists;
            set => SetProperty(ref playlists, value);
        }

        public PlaylistsViewModel(CommandBarViewModel commandBarViewModel, IRegionManager regionManager, PartitionService partitionService, PlaylistService playlistService)
        {
            this.commandBarViewModel = commandBarViewModel;
            this.regionManager = regionManager;
            this.partitionService = partitionService;
            this.playlistService = playlistService;

            Playlists = new ObservableCollection<PlaylistItemViewModel>();

            commandBarViewModel.AddPartitionsIntoPlaylistCommand = new DelegateCommand(AddPartitionsToPlaylist, CanAddPartitionsToPlaylist);
            commandBarViewModel.RemovePartitionsFromPlaylistCommand = new DelegateCommand(RemovePartitionsFromPlaylist, CanRemovePartitionsFromPlaylist);
            commandBarViewModel.DeletePlaylistCommand = new DelegateCommand(DeletePlaylist, CanDeletePlaylist);
            Playlists.CollectionChanged += Playlists_CollectionChanged;
        }

        private bool CanAddPartitionsToPlaylist()
        {
            return Playlists.SelectMany(pl => pl.Partitions).Any(p => p.IsSelected);
        }

        private bool CanRemovePartitionsFromPlaylist()
        {
            return Playlists.SelectMany(pl => pl.Partitions).Any(p => p.IsSelected);
        }

        private bool CanDeletePlaylist()
        {
            return Playlists.Any(pl => pl.IsSelected);
        }

        private void AddPartitionsToPlaylist()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private void RemovePartitionsFromPlaylist()
        {
            try
            {
                var selectedPartitionItems = Playlists.SelectMany(pl => pl.Partitions).Where(p => p.IsSelected).ToList();
                var selectedPartitions = selectedPartitionItems.Select(p => p.Partition).ToList();
                var selectedPlaylistItem = selectedPartitionItems.FirstOrDefault().Playlist;
                var selectedPlaylist = selectedPlaylistItem.Playlist;
                playlistService.RemovePartitionsFromPlaylist(selectedPartitions, selectedPlaylist);
                selectedPartitionItems.ForEach(p => selectedPlaylistItem.Partitions.Remove(p));
                commandBarViewModel.RemovePartitionsFromPlaylistCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {

            }
        }

        private void DeletePlaylist()
        {
            try
            {
                var selectedPlaylist = Playlists.SingleOrDefault(pl => pl.IsSelected);
                playlistService.DeletePlaylist(selectedPlaylist.Playlist);
                Playlists.Remove(selectedPlaylist);
                commandBarViewModel.DeletePlaylistCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {

            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                Playlists.Clear();
                Playlists.AddRange(playlistService.GetPlaylists()
                    .Select(p => new PlaylistItemViewModel(p,
                        commandBarViewModel.AddPartitionsIntoPlaylistCommand,
                        commandBarViewModel.RemovePartitionsFromPlaylistCommand,
                        commandBarViewModel.DeletePlaylistCommand)));
            }
            catch (Exception ex)
            {

            }
        }

        private void Playlists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (PlaylistItemViewModel playlistItem in e.NewItems)
                {
                    playlistItem.PropertyChanged += PlaylistItem_PropertyChanged;
                    playlistItem.PartitionSelected += PlaylistItem_PartitionSelected;
                }
            }

            if (e.OldItems != null)
            {
                foreach (PlaylistItemViewModel playlistItem in e.OldItems)
                {
                    playlistItem.PropertyChanged -= PlaylistItem_PropertyChanged;
                    playlistItem.PartitionSelected -= PlaylistItem_PartitionSelected;
                }
            }
        }

        private void PlaylistItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var playlistItem = sender as PlaylistItemViewModel;

            if (e.PropertyName == "IsSelected" && playlistItem.IsSelected)
            {
                Playlists
                    .Where(pl => pl.Playlist.PlaylistId != playlistItem.Playlist.PlaylistId)
                    .ToList()
                    .ForEach(pl => pl.IsSelected = !playlistItem.IsSelected);
            }
        }

        private void PlaylistItem_PartitionSelected(object sender, EventArgs e)
        {
            var selectedPlaylist = sender as PlaylistItemViewModel;

            var playlistToUnselect = Playlists.Where(pl => pl.Playlist.PlaylistId != selectedPlaylist.Playlist.PlaylistId);

            foreach (var playlist in playlistToUnselect)
            {
                playlist.IsSelected = false;
                playlist.Partitions.ToList().ForEach(p => p.IsSelected = false);
            }
        }
    }
}