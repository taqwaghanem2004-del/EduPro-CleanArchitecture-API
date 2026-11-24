using EduPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPro.Infrastructure.Configurations
{
    public class FavoriteCollectionConfiguration : IEntityTypeConfiguration<FavoriteCollection>
    {
        public void Configure(EntityTypeBuilder<FavoriteCollection> builder)
        {
            builder.HasKey(x => new { x.UserId, x.CollectionId });

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Collection)
                .WithMany()
                .HasForeignKey(x => x.CollectionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
