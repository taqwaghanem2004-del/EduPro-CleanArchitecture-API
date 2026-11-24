namespace EduPro.Data.AppMetaData
{
    public static class Router
    {
        private const string Root = "api";
        private const string Version = "v1";
        private const string Rule = Root + "/" + Version + "/";

        public static class Authentication
        {
            private const string Prefix = Rule + "Authentication/";

            public const string Register = Prefix + "Register";
            public const string SignIn = Prefix + "SignIn";
            public const string ConfirmEmail = Prefix + "ConfirmEmail";
            public const string RefreshToken = Prefix + "RefreshToken";
            public const string SendResetPasswordCode = Prefix + "SendResetPasswordCode";
            public const string ConfirmResetPassword = Prefix + "ConfirmResetPassword";
            public const string ResetPassword = Prefix + "ResetPassword";
            public const string GoogleSignIn = Prefix + "GoogleSignIn";
        }
        public static class CategoryRouting
        {
            private const string Prefix = "/api/v1/Category/";
            public const string GetList = Prefix + "GetList";
            public const string GetById = Prefix + "GetById/{id}";
            public const string Create = Prefix + "Create";
            public const string Update = Prefix + "Update";
            public const string Delete = Prefix + "Delete/{id}";
        }
        public static class AuthorizationRouting
        {
            public const string Prefix = "/Api/Authorization";

            public const string GetRolesList = Prefix + "/GetRolesList";
            public const string GetRoleById = Prefix + "/GetRoleById/{id}";
            public const string AddRole = Prefix + "/AddRole";
            public const string EditRole = Prefix + "/EditRole";
            public const string DeleteRole = Prefix + "/DeleteRole/{id}";
            public const string ManageUserRoles = Prefix + "/ManageUserRoles/{userId}";
            public const string UpdateUserRoles = Prefix + "/UpdateUserRoles";
            public const string ManageUserClaims = Prefix + "/ManageUserClaims/{userId}";
            public const string UpdateUserClaims = Prefix + "/UpdateUserClaims";
            public const string GetUsersList = Prefix + "/GetUsersList";
            public const string BanUser = Prefix + "/BanUser/{userId}";
            public const string UnbanUser = Prefix + "/UnbanUser/{userId}";

        }
        public static class CourseRouting
        {
            public const string Prefix = "/Api/Course";

            public const string GetList = Prefix + "/GetList/{categoryId}";
            public const string GetById = Prefix + "/GetById/{id}";
            public const string Create = Prefix + "/Create";
            public const string Update = Prefix + "/Update";
            public const string Delete = Prefix + "/Delete/{id}";
        }
        public static class EduFileRouting
        {
            public const string Prefix = "/Api/EduFile";
            public const string GetList = Prefix + "/GetList/{status}";
            public const string GetById = Prefix + "/GetById/{id}";
            public const string Upload = Prefix + "/Upload";
            public const string Approve = Prefix + "/Approve/{id}";
            public const string Reject = Prefix + "/Reject/{id}";
            public const string StudentDelete = Prefix + "/StudentDelete/{id}";
            public const string GetByCourseId = Prefix + "/GetByCourseId/{courseId}";
            public const string Download = Prefix + "/Download/{id}";
            public const string Search = Prefix + "/Search";
            public const string GetMyFiles = Prefix + "/GetMyFiles";
            public const string UpdateMyFile = Prefix + "/UpdateMyFile";
            public const string Publish = Prefix + "/Publish/{id}";
            public const string Unpublish = Prefix + "/Unpublish/{id}";
            public const string AdminDelete = Prefix + "/AdminDelete/{id}";
            public const string AskQuestion = "Api/EduFile/Ask";


        }
        public static class StatsRouting
        {
            public const string Prefix = Rule + "Stats/";
            public const string GetStats = Prefix + "GetStats";
            public const string GetAdminStats = Prefix + "GetAdminStats";
        }
        public static class FavoriteRouting
        {
            public const string Prefix = "Favorite";
            public const string GetList = Prefix + "/GetList";
            public const string Add = Prefix + "/Add/{eduFileId}";
            public const string Delete = Prefix + "/Delete/{eduFileId}";
            public const string AddCollectionToFavorites = Prefix + "/AddCollection/{collectionId}";
            public const string RemoveCollectionFromFavorites = Prefix + "/RemoveCollection/{collectionId}";
            public const string GetFavoriteCollections = Prefix + "/GetCollections";
        }
        public static class CollectionRouting
        {
            public const string Prefix = Rule + "Collection";
            public const string GetById = Prefix + "/GetById/{id}";
            public const string Update = Prefix + "/Update";
            public const string Delete = Prefix + "/Delete/{id}";
            public const string AddFile = Prefix + "/AddFile";
            public const string RemoveFile = Prefix + "/RemoveFile";
            public const string Reorder = Prefix + "/Reorder";
            public const string Publish = Prefix + "/Publish/{collectionId}";
            public const string Unpublish = Prefix + "/Unpublish/{collectionId}";
            public const string GetPublicCollections = Prefix + "/GetPublicCollections";
            public const string Copy = Prefix + "/Copy/{collectionId}";
            public const string CreateProfile = Prefix + "/CreateProfile";
            public const string CreateLibrary = Prefix + "/CreateLibrary";
            public const string GetProfileCollections = Prefix + "/GetProfileCollections";
            public const string GetLibraryCollections = Prefix + "/GetLibraryCollections";
            public const string GetByCourseId = Prefix + "/GetByCourseId/{courseId}";

        }
        public static class ProfileRouting
        {
            public const string Prefix = "/Api/Profile";
            public const string GetMyProfile = Prefix + "/GetMyProfile";
            public const string GetPublicProfile = Prefix + "/GetPublicProfile/{userId}";
            public const string UpdateProfile = Prefix + "/UpdateProfile";
            public const string ConfirmEmailChange = Prefix + "/ConfirmEmailChange";
            public const string ChangePassword = Prefix + "/ChangePassword";

        }
       

    }
}
