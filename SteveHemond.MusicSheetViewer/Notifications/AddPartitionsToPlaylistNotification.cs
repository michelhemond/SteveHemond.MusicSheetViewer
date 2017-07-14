using Prism.Interactivity.InteractionRequest;
using SteveHemond.MusicSheetViewer.Data;
using SteveHemond.MusicSheetViewer.ViewModels.Partitions;
using SteveHemond.MusicSheetViewer.ViewModels.Playlists;
using System.Collections.Generic;

namespace SteveHemond.MusicSheetViewer.Notifications
{
    public class AddPartitionsToPlaylistNotification : Confirmation
    {
        public List<PartitionItemViewModel> PartitionItems { get; set; }

        public PlaylistItemViewModel PlaylistItem { get; set; }
    }
}