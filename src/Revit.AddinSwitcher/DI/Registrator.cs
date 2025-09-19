using Microsoft.Extensions.DependencyInjection;
using Revit.AddinSwitcher.ViewModels;

namespace Revit.AddinSwitcher.DI;

internal static class Registrator
{
    public static IServiceCollection AddViews(this IServiceCollection services) 
        => services
        ;

    public static IServiceCollection AddViewModels(this IServiceCollection services) 
        => services
            .AddSingleton<MainWindowViewModel>()
        ;

    public static IServiceCollection AddServices(this IServiceCollection services) 
        => services
        ;
}
