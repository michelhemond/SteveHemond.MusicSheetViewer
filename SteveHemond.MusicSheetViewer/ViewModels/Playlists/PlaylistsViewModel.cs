using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using SteveHemond.MusicSheetViewer.Data;
using SteveHemond.MusicSheetViewer.Notifications;
using SteveHemond.MusicSheetViewer.Services;
using SteveHemond.MusicSheetViewer.ViewModels.Partitions;
using System;
using System.Collections.Generic;
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

        public InteractionRequest<AddPartitionsToPlaylistNotification> AddPartitionsToPlaylistInteractionRequest { get; set; }

        private ObservableCollection<PlaylistItemViewModel> playlistItems;
        public ObservableCollection<PlaylistItemViewModel> PlaylistItems
        {
            get => playlistItems;
            set => SetProperty(ref playlistItems, value);
        }

        public PlaylistsViewModel(CommandBarViewModel commandBarViewModel, IRegionManager regionManager, PartitionService partitionService, PlaylistService playlistService)
        {
            this.commandBarViewModel = commandBarViewModel;
            this.regionManager = regionManager;
            this.partitionService = partitionService;
            this.playlistService = playlistService;

            PlaylistItems = new ObservableCollection<PlaylistItemViewModel>();
            AddPartitionsToPlaylistInteractionRequest = new InteractionRequest<AddPartitionsToPlaylistNotification>();
            commandBarViewModel.AddPartitionsToPlaylistCommand = new DelegateCommand(AddPartitionsToPlaylist, CanAddPartitionsToPlaylist);
            commandBarViewModel.RemovePartitionsFromPlaylistCommand = new DelegateCommand(RemovePartitionsFromPlaylist, CanRemovePartitionsFromPlaylist);
            commandBarViewModel.DeletePlaylistCommand = new DelegateCommand(DeletePlaylist, CanDeletePlaylist);
            PlaylistItems.CollectionChanged += Playlists_CollectionChanged;
        }

        private bool CanAddPartitionsToPlaylist()
        {
            return PlaylistItems.SelectMany(pl => pl.Partitions).Any(p => p.IsSelected);
        }

        private bool CanRemovePartitionsFromPlaylist()
        {
            return PlaylistItems.SelectMany(pl => pl.Partitions).Any(p => p.IsSelected);
        }

        private bool CanDeletePlaylist()
        {
            return PlaylistItems.Any(pl => pl.IsSelected);
        }

        private void AddPartitionsToPlaylist()
        {
            try
            {
                var addPartitionsIntoPlaylistNotification = new AddPartitionsToPlaylistNotification()
                {
                    Title = "Ajouter des partitions à une liste de lecture",
                    Content = string.Empty,
                    PlaylistItem = PlaylistItems.SingleOrDefault(pl => pl.IsSelected)
                };

                AddPartitionsToPlaylistInteractionRequest.Raise(addPartitionsIntoPlaylistNotification, returned =>
                {
                    if (returned != null && returned.Confirmed)
                    {
                        var playlist = addPartitionsIntoPlaylistNotification.PlaylistItem;

                        if (playlist.Partitions == null)
                        {
                            playlist.Partitions = new ObservableCollection<PartitionItemViewModel>();
                        }

                        playlistService.AddPartitionsToPlaylist(
                            playlist.Playlist, 
                            addPartitionsIntoPlaylistNotification.PartitionItems.Select(p => p.Partition).ToList());
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void RemovePartitionsFromPlaylist()
        {
            try
            {
                var selectedPartitionItems = PlaylistItems.SelectMany(pl => pl.Partitions).Where(p => p.IsSelected).ToList();
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
                var selectedPlaylist = PlaylistItems.SingleOrDefault(pl => pl.IsSelected);
                playlistService.DeletePlaylist(selectedPlaylist.Playlist);
                PlaylistItems.Remove(selectedPlaylist);
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
                PlaylistItems.Clear();
                PlaylistItems.AddRange(playlistService.GetPlaylists()
                    .Select(p => new PlaylistItemViewModel(p,
                        commandBarViewModel.AddPartitionsToPlaylistCommand,
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
                PlaylistItems
                    .Where(pl => pl.Playlist.PlaylistId != playlistItem.Playlist.PlaylistId)
                    .ToList()
                    .ForEach(pl => pl.IsSelected = !playlistItem.IsSelected);
            }
        }

        private void PlaylistItem_PartitionSelected(object sender, EventArgs e)
        {
            var selectedPlaylist = sender as PlaylistItemViewModel;

            var playlistToUnselect = PlaylistItems.Where(pl => pl.Playlist.PlaylistId != selectedPlaylist.Playlist.PlaylistId);

            foreach (var playlist in playlistToUnselect)
            {
                playlist.IsSelected = false;
                playlist.Partitions.ToList().ForEach(p => p.IsSelected = false);
            }
        }
    }
}