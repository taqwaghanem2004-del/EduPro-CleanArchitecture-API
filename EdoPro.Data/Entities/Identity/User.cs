using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace EduPro.Data.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            UserRefreshTokens = new HashSet<UserRefreshToken>();
        }

        public string FullName { get; set; }

        [EncryptColumn]
        public string? OtpCode { get; set; }
        public string? ProfilePicture { get; set; }

        public bool IsBlocked { get; set; } = false;

        [InverseProperty(nameof(UserRefreshToken.User))]
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
