using EduPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPro.Infrastructure.Configurations
{
    public class EduFileConfiguration : IEntityTypeConfiguration<EduFile>
    {
        public void Configure(EntityTypeBuilder<EduFile> builder)
        {
            builder.HasOne(e => e.Course)
                   .WithMany(c => c.EduFiles)
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.UploadedBy)
                   .WithMany()
                   .HasForeignKey(e => e.UploadedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
