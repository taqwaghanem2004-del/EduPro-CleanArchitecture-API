using EduPro.Data.Entities.Identity;

namespace EduPro.Data.Entities
{
    public class FavoriteCollection
    {
        public int UserId { get; set; }
        public int CollectionId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public virtual User? User { get; set; }
        public virtual Collection? Collection { get; set; }
    }
}
