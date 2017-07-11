using Prism.Commands;
using Prism.Mvvm;
using SteveHemond.MusicSheetViewer.Data;
using SteveHemond.MusicSheetViewer.ViewModels.Partitions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.ComponentModel;

namespace SteveHemond.MusicSheetViewer.ViewModels.Playlists
{
    public class PlaylistItemViewModel : BindableBase
    {
        private readonly DelegateCommand addPartitionsToPlaylistCommand;

        private readonly DelegateCommand removePartitionsFromPlaylistCommand;

        private readonly DelegateCommand deletePlaylistCommand;

        public event EventHandler PartitionSelected;

        private Playlist playlist;
        public Playlist Playlist
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

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private ObservableCollection<PartitionItemViewModel> partitions;
        public ObservableCollection<PartitionItemViewModel> Partitions
        {
            get => partitions;
            set => SetProperty(ref partitions, value);
        }

        private int partitionCount;
        public int PartitionCount
        {
            get => partitionCount;
            set => SetProperty(ref partitionCount, value);
        }

        public string PartitionCountText
        {
            get => $"{PartitionCount} partitions";
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value)
                {
                    return;
                }

                SetProperty(ref isSelected, value);

                foreach(var partition in Partitions)
                {
                    partition.IsSelected = value;
                }

                deletePlaylistCommand?.RaiseCanExecuteChanged();
            }
        }

        public PlaylistItemViewModel(
            Playlist playlist, 
            DelegateCommand addPartitionsToPlaylistCommand, 
            DelegateCommand removePartitionsFromPlaylistCommand, 
            DelegateCommand deletePlaylistCommand)
        {
            this.addPartitionsToPlaylistCommand = addPartitionsToPlaylistCommand;
            this.removePartitionsFromPlaylistCommand = removePartitionsFromPlaylistCommand;
            this.deletePlaylistCommand = deletePlaylistCommand;

            Playlist = playlist;
            DisplayName = playlist.DisplayName;
            Description = playlist.Description;
            Partitions = new ObservableCollection<PartitionItemViewModel>();
            Partitions.CollectionChanged += Partitions_CollectionChanged;
            Partitions.AddRange(Playlist.Partitions
                .Select(p => new PartitionItemViewModel(p, addPartitionsToPlaylistCommand, removePartitionsFromPlaylistCommand)).ToList());
            PartitionCount = playlist.Partitions.Count;
        }

        private void Partitions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (PartitionItemViewModel partitionItem in e.NewItems)
                {
                    partitionItem.PropertyChanged += PartitionItem_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (PartitionItemViewModel partitionItem in e.OldItems)
                {
                    partitionItem.PropertyChanged -= PartitionItem_PropertyChanged;
                }
            }
        }

        private void PartitionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var partitionItem = sender as PartitionItemViewModel;

            if (e.PropertyName == "IsSelected")
            {
                if (partitionItem.IsSelected)
                {
                    PartitionSelected?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}