using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using SteveHemond.MusicSheetViewer.Data;
using SteveHemond.MusicSheetViewer.Notifications;
using SteveHemond.MusicSheetViewer.Services;
using SteveHemond.MusicSheetViewer.ViewModels.Playlists;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SteveHemond.MusicSheetViewer.ViewModels.Partitions
{
    public class AddPartitionsToPlaylistViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly PlaylistService playlistService;

        private AddPartitionsToPlaylistNotification addPartitionsToPlaylistNotification;

        private ObservableCollection<PlaylistItemViewModel> playlists;
        public ObservableCollection<PlaylistItemViewModel> Playlists
        {
            get => playlists;
            set => SetProperty(ref playlists, value);
        }

        private PlaylistItemViewModel selectedPlaylist;
        public PlaylistItemViewModel SelectedPlaylist
        {
            get => selectedPlaylist;
            set
            {
                SetProperty(ref selectedPlaylist, value);
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        private string playlistName;
        public string PlaylistName
        {
            get => playlistName;
            set
            {
                SetProperty(ref playlistName, value);
                AddPlaylistCommand.RaiseCanExecuteChanged();
            }
        }

        public INotification Notification
        {
            get { return addPartitionsToPlaylistNotification; }

            set
            {
                if (value is AddPartitionsToPlaylistNotification)
                {
                    addPartitionsToPlaylistNotification = value as AddPartitionsToPlaylistNotification;
                    RaisePropertyChanged(nameof(Notification));
                    GetPlaylists();
                }
            }
        }
        
        public Action FinishInteraction { get; set; }

        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand ConfirmCommand { get; private set; }

        public DelegateCommand AddPlaylistCommand { get; private set; }

        public AddPartitionsToPlaylistViewModel(PlaylistService playlistService)
        {
            this.playlistService = playlistService;
            Playlists = new ObservableCollection<PlaylistItemViewModel>();
            CancelCommand = new DelegateCommand(CancelInteraction);
            ConfirmCommand = new DelegateCommand(ConfirmInteraction, () => SelectedPlaylist != null);
            AddPlaylistCommand = new DelegateCommand(AddPlaylist, () => !string.IsNullOrEmpty(PlaylistName));
        }

        private void AddPlaylist()
        {
            try
            {
                var playlist = new Playlist { DisplayName = PlaylistName };
                playlistService.AddPlaylist(playlist);
                Playlists.Add(new PlaylistItemViewModel(playlist));
                PlaylistName = string.Empty;
                SelectedPlaylist = new PlaylistItemViewModel(playlist);
            }
            catch (Exception ex)
            {
                   
            }
        }

        private void GetPlaylists()
        {
            Playlists.Clear();
            Playlists.AddRange(playlistService.GetPlaylists().Select(pl => new PlaylistItemViewModel(pl)));
        }

        private void ConfirmInteraction()
        {
            addPartitionsToPlaylistNotification.Playlist = SelectedPlaylist;
            addPartitionsToPlaylistNotification.Confirmed = true;
            FinishInteraction();
        }

        private void CancelInteraction()
        {
            addPartitionsToPlaylistNotification.Confirmed = false;
            FinishInteraction();
        }
    }
}