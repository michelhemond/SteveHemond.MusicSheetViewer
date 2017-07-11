using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SteveHemond.MusicSheetViewer.Data;
using SteveHemond.MusicSheetViewer.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Interactivity.InteractionRequest;
using SteveHemond.MusicSheetViewer.Notifications;

namespace SteveHemond.MusicSheetViewer.ViewModels.Partitions
{
    public class PartitionsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private readonly PartitionService partitionService;

        private readonly PlaylistService playlistService;

        private readonly CommandBarViewModel commandBarViewModel;

        public InteractionRequest<AddPartitionsToPlaylistNotification> AddPartitionsToPlaylistInteractionRequest { get; set; }

        private ObservableCollection<PartitionItemViewModel> partitions;
        public ObservableCollection<PartitionItemViewModel> Partitions
        {
            get => partitions;
            set => SetProperty(ref partitions, value);
        }

        private PartitionItemViewModel selectedPartition;
        public PartitionItemViewModel SelectedPartition
        {
            get => selectedPartition;
            set => SetProperty(ref selectedPartition, value);
        }

        public bool IsSelectionActive
        {
            get => Partitions.Any(p => p.IsSelected);
        }

        public PartitionsViewModel(CommandBarViewModel commandBarViewModel, IRegionManager regionManager, PartitionService partitionService, PlaylistService playlistService)
        {
            this.commandBarViewModel = commandBarViewModel;
            this.regionManager = regionManager;
            this.partitionService = partitionService;
            this.playlistService = playlistService;

            AddPartitionsToPlaylistInteractionRequest = new InteractionRequest<AddPartitionsToPlaylistNotification>();
            Partitions = new ObservableCollection<PartitionItemViewModel>();
            commandBarViewModel.AddToPlaylistCommand = new DelegateCommand(AddToPlaylist, CanAddToPlaylist);
        }

        private bool CanAddToPlaylist()
        {
            return IsSelectionActive;
        }

        private void AddToPlaylist()
        {
            var addPartitionsToPlaylistNotification = new AddPartitionsToPlaylistNotification()
            {
                Title = "Ajouter des partitions à une liste de lecture",
                Content = string.Empty,
                Partitions = Partitions.Where(p => p.IsSelected).Select(p => p.Partition).ToList()
            };

            AddPartitionsToPlaylistInteractionRequest.Raise(addPartitionsToPlaylistNotification, returned =>
            {
                if (returned != null && returned.Confirmed)
                {
                    var playlist = addPartitionsToPlaylistNotification.Playlist;

                    if (playlist.Partitions == null)
                    {
                        playlist.Partitions = new List<Partition>();
                    }

                    playlistService.AddPartitionsToPlaylist(playlist, addPartitionsToPlaylistNotification.Partitions);
                }
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await GetPartitions();
        }

        public async Task GetPartitions()
        {
            Partitions.Clear();
            var partitions = await partitionService.GetPartitions();
            Partitions.AddRange(partitions.Select(p => new PartitionItemViewModel(p, commandBarViewModel.AddToPlaylistCommand, null)));
        }
    }
}