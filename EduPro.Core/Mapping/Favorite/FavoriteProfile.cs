using AutoMapper;

namespace EduPro.Core.Mapping.Favorite
{
    public partial class FavoriteProfile : Profile
    {

        public FavoriteProfile()
        {
            GetFavoriteListMapping();
            GetFavoriteCollectionListMapping();
        }
    }
}