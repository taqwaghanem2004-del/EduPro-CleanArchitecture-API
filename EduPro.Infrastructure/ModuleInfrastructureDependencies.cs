using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.InfrastructureBases;
using EduPro.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EduPro.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IEduFileRepository, EduFileRepository>();
            services.AddTransient<IFavoriteRepository, FavoriteRepository>();
            services.AddTransient<ICollectionFileRepository, CollectionFileRepository>();
            services.AddTransient<ICollectionRepository, CollectionRepository>();
           
            services.AddTransient<IFavoriteCollectionRepository, FavoriteCollectionRepository>();
            services.AddTransient<IUserActivityRepository, UserActivityRepository>();


            return services;
        }
    }
}
