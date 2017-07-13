using SteveHemond.MusicSheetViewer.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SteveHemond.MusicSheetViewer.Services
{
    public class PlaylistService
    {
        public void AddPlaylist(Playlist playlist)
        {
            using (var dbContext = new MusicSheetViewerContext())
            {
                dbContext.Playlists.Add(playlist);
                dbContext.SaveChanges();
            }
        }

        public List<Playlist> GetPlaylists()
        {
            using (var dbContext = new MusicSheetViewerContext())
            {
                return dbContext.Playlists.Include(p => p.Partitions).ToList();
            }
        }

        public void AddPartitionsToPlaylist(Playlist playlist, List<Partition> partitions)
        {
            using (var dbContext = new MusicSheetViewerContext())
            {
                dbContext.Playlists.Attach(playlist);

                foreach(var partition in partitions)
                {
                    if (playlist.Partitions == null && playlist.Partitions.Any(p => p.PartitionId == partition.PartitionId))
                    {
                        continue;
                    }

                    dbContext.Partitions.Attach(partition);
                    playlist.Partitions.Add(partition);
                }
                
                dbContext.SaveChanges();
            }
        }

        public void RemovePartitionsFromPlaylist(List<Partition> partitions, Playlist playlist)
        {
            using (var dbContext = new MusicSheetViewerContext())
            {
                dbContext.Playlists.Attach(playlist);
                partitions.ForEach(p => dbContext.Partitions.Attach(p));

                foreach (var partition in partitions)
                {
                    playlist.Partitions.Remove(partition);
                }

                dbContext.SaveChanges();
            }
        }

        public void DeletePlaylist(Playlist playlist)
        {
            using (var dbContext = new MusicSheetViewerContext())
            {
                dbContext.Playlists.Attach(playlist);
                dbContext.Playlists.Remove(playlist);
                dbContext.SaveChanges();
            }
        }
    }
}