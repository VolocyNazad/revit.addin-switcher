using Microsoft.Extensions.DependencyInjection;
using Revit.AddinSwitcher.Abstractions.Services;
using Revit.AddinSwitcher.Abstractions.ViewModels;
using Revit.AddinSwitcher.Services;
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

            .AddSingleton<ITargetAddinDirectoryPathContainerViewModel>(provider => provider.GetRequiredService<TargetAddinDirectoryPathContainerViewModel>())
            .AddSingleton<TargetAddinDirectoryPathContainerViewModel>()
            .AddSingleton<IAddinPathContainerViewModel>(provider => provider.GetRequiredService<AddinPathContainerViewModel>())
            .AddSingleton<AddinPathContainerViewModel>()
            .AddSingleton<ActivatedAddinPathContainerViewModel>()
            .AddSingleton<DeactivatedAddinPathContainerViewModel>()

            .AddSingleton<RefreshAddinPathContainerViewModel>()

            .AddSingleton<ActivateAddinsViewModel>()
        ;

    public static IServiceCollection AddServices(this IServiceCollection services) 
        => services
            .AddTransient<IDebounceDispatcher, DebounceDispatcher>()

            .AddTransient<IActivateAddinService, ActivateAddinService>()
        ;
}
