using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteveHemond.MusicSheetViewer.Data
{
    [Table("Partition")]
    public class Partition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PartitionId { get; set; }

        public string FileName { get; set; }

        public int PageCount { get; set; }

        [MaxLength]
        public byte[] Thumbnail { get; set; }

        public List<Page> Pages { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}