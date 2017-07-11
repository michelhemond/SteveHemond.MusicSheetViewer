using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteveHemond.MusicSheetViewer.Data
{
    public class MusicSheetViewerContext : DbContext
    {
        public MusicSheetViewerContext() : base("MusicSheetViewerConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Partition>()
                        .HasMany(s => s.Playlists)
                        .WithMany(c => c.Partitions)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("PartitionId");
                            cs.MapRightKey("PlaylistId");
                            cs.ToTable("PlaylistPartition");
                        });
        }

        public DbSet<Partition> Partitions { get; set; }

        public DbSet<Playlist> Playlists { get; set; }
    }
}