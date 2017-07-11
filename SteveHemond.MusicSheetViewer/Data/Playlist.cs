using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SteveHemond.MusicSheetViewer.Data
{
    [Table("Playlist")]
    public class Playlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlaylistId { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Partition> Partitions { get; set; }
    }
}