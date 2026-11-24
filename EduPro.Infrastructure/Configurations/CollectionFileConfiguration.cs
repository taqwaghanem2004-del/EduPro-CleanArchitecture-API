using EduPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPro.Infrastructure.Configurations
{
    public class CollectionFileConfiguration : IEntityTypeConfiguration<CollectionFile>
    {
        public void Configure(EntityTypeBuilder<CollectionFile> builder)
        {
            builder.HasKey(x => new { x.CollectionId, x.EduFileId });

            builder.HasOne(cf => cf.Collection)
                   .WithMany(c => c.CollectionFiles)
                   .HasForeignKey(cf => cf.CollectionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cf => cf.EduFile)
                   .WithMany(f => f.CollectionFiles)
                   .HasForeignKey(cf => cf.EduFileId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
