using EduPro.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduPro.Data.Entities
{
    public class Favorite
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int EduFileId { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(EduFileId))]
        [InverseProperty("Favorites")]
        public virtual EduFile? EduFile { get; set; }
    }
}
