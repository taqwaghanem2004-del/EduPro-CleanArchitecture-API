using AutoMapper;

namespace EduPro.Core.Mapping.Collection
{
    public partial class CollectionProfile : Profile
    {
        public CollectionProfile()
        {
            GetCollectionListMapping();
            GetCollectionByIdMapping();
        }
    }
}
