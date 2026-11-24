using EduPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPro.Infrastructure.Configurations
{

    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(x => new { x.UserId, x.EduFileId });

            builder.HasOne(f => f.User)
                   .WithMany()
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.EduFile)
                   .WithMany(e => e.Favorites)
                   .HasForeignKey(f => f.EduFileId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
