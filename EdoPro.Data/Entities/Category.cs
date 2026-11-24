using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduPro.Data.Entities
{
    public class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
