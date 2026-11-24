using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduPro.Data.Entities
{
    public class Course
    {
        public Course()
        {
            EduFiles = new HashSet<EduFile>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Courses")]
        public virtual Category? Category { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<EduFile> EduFiles { get; set; }
    }

}
