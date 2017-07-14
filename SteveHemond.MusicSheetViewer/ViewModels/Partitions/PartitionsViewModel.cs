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

        private ObservableCollection<PartitionItemViewModel> partitionItems;
        public ObservableCollection<PartitionItemViewModel> PartitionItems
        {
            get => partitionItems;
            set => SetProperty(ref partitionItems, value);
        }

        private PartitionItemViewModel selectedPartitionItem;
        public PartitionItemViewModel SelectedPartitionItem
        {
            get => selectedPartitionItem;
            set => SetProperty(ref selectedPartitionItem, value);
        }

        public bool IsSelectionActive
        {
            get => PartitionItems.Any(p => p.IsSelected);
        }

        public PartitionsViewModel(CommandBarViewModel commandBarViewModel, IRegionManager regionManager, PartitionService partitionService, PlaylistService playlistService)
        {
            this.commandBarViewModel = commandBarViewModel;
            this.regionManager = regionManager;
            this.partitionService = partitionService;
            this.playlistService = playlistService;

            AddPartitionsToPlaylistInteractionRequest = new InteractionRequest<AddPartitionsToPlaylistNotification>();
            PartitionItems = new ObservableCollection<PartitionItemViewModel>();
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
                PartitionItems = PartitionItems.Where(p => p.IsSelected).ToList()
            };

            AddPartitionsToPlaylistInteractionRequest.Raise(addPartitionsToPlaylistNotification, returned =>
            {
                if (returned != null && returned.Confirmed)
                {
                    var playlistItem = addPartitionsToPlaylistNotification.PlaylistItem;

                    if (playlistItem.Playlist.Partitions == null)
                    {
                        playlistItem.Playlist.Partitions = new List<Partition>();
                    }

                    playlistService.AddPartitionsToPlaylist(
                        playlistItem.Playlist, 
                        addPartitionsToPlaylistNotification.PartitionItems.Select(p => p.Partition).ToList());
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
            PartitionItems.Clear();
            var partitions = await partitionService.GetPartitions();
            PartitionItems.AddRange(partitions.Select(p => new PartitionItemViewModel(p, null, commandBarViewModel.AddToPlaylistCommand, null)));
        }
    }
}