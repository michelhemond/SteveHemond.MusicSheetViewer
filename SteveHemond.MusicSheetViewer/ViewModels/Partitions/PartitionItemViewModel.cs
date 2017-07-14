using Prism.Mvvm;
using SteveHemond.MusicSheetViewer.Data;
using System.Windows.Media.Imaging;
using SteveHemond.MusicSheetViewer.Helpers;
using Prism.Commands;
using SteveHemond.MusicSheetViewer.ViewModels.Playlists;

namespace SteveHemond.MusicSheetViewer.ViewModels.Partitions
{
    public class PartitionItemViewModel : BindableBase
    {
        private readonly DelegateCommand addPartitionsToPlaylistCommand;

        private readonly DelegateCommand removePartitionsFromPlaylistCommand;

        private Partition partition;
        public Partition Partition
        {
            get => partition;
            set => SetProperty(ref partition, value);
        }

        private PlaylistItemViewModel playlist;
        public PlaylistItemViewModel Playlist
        {
            get => playlist;
            set => SetProperty(ref playlist, value);
        }

        private string displayName;
        public string DisplayName
        {
            get => displayName;
            set => SetProperty(ref displayName, value);
        }

        private BitmapImage thumbnail;
        public BitmapImage Thumbnail
        {
            get => thumbnail;
            set => SetProperty(ref thumbnail, value);
        }

        private int pageCount;
        public int PageCount
        {
            get => pageCount;
            set => SetProperty(ref pageCount, value);
        }

        public string PageCountText => $"{PageCount} pages";

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                SetProperty(ref isSelected, value);
                addPartitionsToPlaylistCommand?.RaiseCanExecuteChanged();
                removePartitionsFromPlaylistCommand?.RaiseCanExecuteChanged();
            }
        }

        public PartitionItemViewModel(Partition partition, PlaylistItemViewModel playlist, DelegateCommand addPartitionsToPlaylistCommand = null, DelegateCommand removePartitionsFromPlaylistCommand = null)
        {
            this.addPartitionsToPlaylistCommand = addPartitionsToPlaylistCommand;
            this.removePartitionsFromPlaylistCommand = removePartitionsFromPlaylistCommand;

            Partition = partition;
            Playlist = playlist;
            DisplayName = partition.FileName;
            Thumbnail = partition.Thumbnail.FromByteArray();
            PageCount = partition.PageCount;
        }
    }
}