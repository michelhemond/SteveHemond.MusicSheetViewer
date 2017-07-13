using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using SteveHemond.MusicSheetViewer.Data;
using SteveHemond.MusicSheetViewer.Notifications;
using SteveHemond.MusicSheetViewer.Services;
using SteveHemond.MusicSheetViewer.ViewModels.Partitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteveHemond.MusicSheetViewer.ViewModels.Playlists
{
    public class AddPartitionsToPlaylistViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly PlaylistService playlistService;

        private AddPartitionsToPlaylistNotification addPartitionsToPlaylistNotification;

        private ObservableCollection<PartitionItemViewModel> partitions;
        public ObservableCollection<PartitionItemViewModel> Partitions
        {
            get => partitions;
            set => SetProperty(ref partitions, value);
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
                }
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand ConfirmCommand { get; private set; }

        public AddPartitionsToPlaylistViewModel(PlaylistService playlistService)
        {
            this.playlistService = playlistService;
            Partitions = new ObservableCollection<PartitionItemViewModel>();
            CancelCommand = new DelegateCommand(CancelInteraction);
            ConfirmCommand = new DelegateCommand(ConfirmInteraction, () => Partitions.Any(p => p.IsSelected));
        }

        private void ConfirmInteraction()
        {
            addPartitionsToPlaylistNotification.Partitions = Partitions.Where(p => p.IsSelected).ToList();
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
