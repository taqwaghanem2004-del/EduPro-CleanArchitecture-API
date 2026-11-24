using System.Security.Claims;

namespace EduPro.Data.Helpers
{
    public static class ClaimsStore
    {
        public static List<Claim> claims = new()
        {
           new Claim("ManageCategories", "false"),
            new Claim("ManageCourses", "false"),
            new Claim("ManageFiles", "false"),
            new Claim("SuperAdmin", "false"),
        };
    }
}
