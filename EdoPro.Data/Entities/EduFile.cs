using EduPro.Data.Entities.Identity;
using EduPro.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduPro.Data.Entities
{
    public class EduFile
    {
        public EduFile()
        {
            Favorites = new HashSet<Favorite>();
            CollectionFiles = new HashSet<CollectionFile>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string FilePath { get; set; }
        public string? AiSummary { get; set; }

        public EduFileType FileType { get; set; }

        public EduFileStatus Status { get; set; } = EduFileStatus.Pending;

        public int DownloadCount { get; set; } = 0;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int UploadedByUserId { get; set; }

        public int? CourseId { get; set; }

       
        [ForeignKey(nameof(UploadedByUserId))]
        public virtual User? UploadedBy { get; set; }

        [ForeignKey(nameof(CourseId))]
        [InverseProperty("EduFiles")]
        public virtual Course? Course { get; set; }

      

        [InverseProperty("EduFile")]
        public virtual ICollection<Favorite> Favorites { get; set; }

        public virtual ICollection<CollectionFile> CollectionFiles { get; set; }
        public string? RequestedCourseName { get; set; }
        public string? RequestedCategoryName { get; set; }
        public bool IsPublished { get; set; } = false;
        public bool IsDeleted { get; set; } = false;


    }
}