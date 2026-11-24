namespace EduPro.Infrastructure.Configurations
{
    using global::EduPro.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace EduPro.Infrastructure.Configurations
    {
        public class CourseConfiguration : IEntityTypeConfiguration<Course>
        {
            public void Configure(EntityTypeBuilder<Course> builder)
            {
                builder.HasOne(x => x.Category)
                    .WithMany(x => x.Courses)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }

}
