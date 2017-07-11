using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteveHemond.MusicSheetViewer.Data
{
    [Table("Page")]
    public class Page
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageId { get; set; }

        public int PageNumber { get; set; }

        [MaxLength]
        public byte[] Image { get; set; }
    }
}