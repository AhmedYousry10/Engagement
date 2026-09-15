using EngagementApi.Application.Auth;
using EngagementApi.Application.Details;
using EngagementApi.Application.Photos;
using EngagementApi.Application.SiteContents;
using Microsoft.Extensions.DependencyInjection;

namespace EngagementApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISiteContentService, SiteContentService>();
        services.AddScoped<IDetailsService, DetailsService>();
        services.AddScoped<IPhotosService, PhotosService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
