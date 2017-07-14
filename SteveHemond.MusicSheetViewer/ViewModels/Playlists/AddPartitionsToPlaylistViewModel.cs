using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using SteveHemond.MusicSheetViewer.Notifications;
using SteveHemond.MusicSheetViewer.Services;
using SteveHemond.MusicSheetViewer.ViewModels.Partitions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SteveHemond.MusicSheetViewer.ViewModels.Playlists
{
    public class AddPartitionsToPlaylistViewModel : BindableBase, IInteractionRequestAware
    {
        private readonly PartitionService partitionService;

        private AddPartitionsToPlaylistNotification addPartitionsToPlaylistNotification;

        private PlaylistItemViewModel playlistItem;

        private ObservableCollection<PartitionItemViewModel> partitionItems;
        public ObservableCollection<PartitionItemViewModel> PartitionItems
        {
            get => partitionItems;
            set => SetProperty(ref partitionItems, value);
        }

        public INotification Notification
        {
            get { return addPartitionsToPlaylistNotification; }

            set
            {
                if (value is AddPartitionsToPlaylistNotification)
                {
                    addPartitionsToPlaylistNotification = value as AddPartitionsToPlaylistNotification;
                    playlistItem = addPartitionsToPlaylistNotification.PlaylistItem;
                    RaisePropertyChanged(nameof(Notification));
                }
            }
        }

        public Action FinishInteraction { get; set; }

        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand ConfirmCommand { get; private set; }

        public AddPartitionsToPlaylistViewModel(PartitionService partitionService)
        {
            this.partitionService = partitionService;
            PartitionItems = new ObservableCollection<PartitionItemViewModel>();
            CancelCommand = new DelegateCommand(CancelInteraction);
            ConfirmCommand = new DelegateCommand(ConfirmInteraction, () => PartitionItems.Any(p => p.IsSelected));
            GetPartitions();
        }

        public async void GetPartitions()
        {
            PartitionItems = new ObservableCollection<PartitionItemViewModel>(
                (await partitionService.GetPartitions())
                .Select(p => new PartitionItemViewModel(p, playlistItem, ConfirmCommand))
                .ToList());
        }

        private void ConfirmInteraction()
        {
            addPartitionsToPlaylistNotification.PartitionItems = PartitionItems.Where(p => p.IsSelected).ToList();
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