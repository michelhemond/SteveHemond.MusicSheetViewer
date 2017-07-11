using Prism.Interactivity.InteractionRequest;
using SteveHemond.MusicSheetViewer.Data;
using System.Collections.Generic;

namespace SteveHemond.MusicSheetViewer.Notifications
{
    public class AddPartitionsToPlaylistNotification : Confirmation
    {
        public List<Partition> Partitions { get; set; }

        public Playlist Playlist { get; set; }
    }
}