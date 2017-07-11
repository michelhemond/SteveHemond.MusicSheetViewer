using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using SteveHemond.MusicSheetViewer.Data;
using SteveHemond.MusicSheetViewer.Notifications;
using SteveHemond.MusicSheetViewer.Services;
using System;
using System.Collections.ObjectModel;

namespace SteveHemond.MusicSheetViewer.ViewModels.Partitions
{
    public class AddPartitionsToPlaylistViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly PlaylistService playlistService;

        private AddPartitionsToPlaylistNotification addPartitionsToPlaylistNotification;

        private ObservableCollection<Playlist> playlists;
        public ObservableCollection<Playlist> Playlists
        {
            get => playlists;
            set => SetProperty(ref playlists, value);
        }

        private Playlist selectedPlaylist;
        public Playlist SelectedPlaylist
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
            Playlists = new ObservableCollection<Playlist>();
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
                Playlists.Add(playlist);
                PlaylistName = string.Empty;
                SelectedPlaylist = playlist;
            }
            catch (Exception ex)
            {
                   
            }
        }

        private void GetPlaylists()
        {
            Playlists.Clear();
            Playlists.AddRange(playlistService.GetPlaylists());
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