using Microsoft.Extensions.DependencyInjection;

namespace Revit.AddinSwitcher.DI;

internal static class Registrator
{
    public static IServiceCollection AddViews(this IServiceCollection services) 
        => services
        ;

    public static IServiceCollection AddViewModels(this IServiceCollection services) 
        => services
        ;

    public static IServiceCollection AddServices(this IServiceCollection services) 
        => services
        ;
}
