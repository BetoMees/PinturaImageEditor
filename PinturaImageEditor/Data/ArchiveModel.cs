using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PinturaImageEditor.Data
{
    public class ArchiveModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }
        [Required]
        public required string Url { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        [NotMapped] public EArchiveStatus Status { get; set; }
        [NotMapped] public string OldUrl { get; set; }
        public enum EArchiveStatus
        {
            Normal,
            New,
            Edited,
            Deleted
        }
    }
}
