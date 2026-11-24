using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Repositories;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Implementations;
using EduPro.Service.AuthServices.Interfaces;
using EduPro.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace EduPro.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<Abstracts.IAuthenticationService, Implementations.AuthenticationService>();
            services.AddTransient<IEmailsService, EmailsService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IEduFileService, EduFileService>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<ICollectionService, CollectionService>();
            services.AddTransient<IFavoriteCollectionRepository, FavoriteCollectionRepository>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IUserActivityService, UserActivityService>();
            services.AddHttpClient<IAiService, AiService>();


            return services;
        }
    }
}
