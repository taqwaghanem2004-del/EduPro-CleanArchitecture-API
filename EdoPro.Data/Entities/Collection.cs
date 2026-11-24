using EduPro.Data.Entities.Identity;
using EduPro.Data.Enums;

namespace EduPro.Data.Entities
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public bool IsPublished { get; set; } = false;
        public CollectionType Type { get; set; } = CollectionType.Library;
        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }
        public virtual ICollection<CollectionFile> CollectionFiles { get; set; } = new HashSet<CollectionFile>();


    }
}
