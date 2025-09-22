using Microsoft.Extensions.DependencyInjection;
using Revit.AddinSwitcher.Abstractions.ViewModels;

namespace Revit.AddinSwitcher.ViewModels;

internal sealed class ViewModelLocator
{
    /// <summary> Главная модель представления </summary>
    public MainWindowViewModel MainWindow => Provider.GetRequiredService<MainWindowViewModel>();

    /// <summary> Контейнер для визуализации коллекции доступных для взаимодействия плагинов </summary>
    public IAddinPathContainerViewModel AddinPathContainer 
        => Provider.GetRequiredService<IAddinPathContainerViewModel>();

    /// <summary> Контейнер для визуализации коллекции доступных для взаимодействия (активных) плагинов </summary>
    public ActivatedAddinPathContainerViewModel ActivatedAddinPathContainer 
        => Provider.GetRequiredService<ActivatedAddinPathContainerViewModel>();

    /// <summary> Контейнер для визуализации коллекции доступных для взаимодействия (не активных) плагинов </summary>
    public DeactivatedAddinPathContainerViewModel DeactivatedAddinPathContainer 
        => Provider.GetRequiredService<DeactivatedAddinPathContainerViewModel>();

    /// <summary> Обновление контейнера для визуализации коллекции доступных для взаимодействия плагинов </summary>
    public RefreshAddinPathContainerViewModel RefreshAddinPathContainer
        => Provider.GetRequiredService<RefreshAddinPathContainerViewModel>();

    /// <summary> Активация/Деактивация плагины </summary>
    public ActivateAddinsViewModel ActivateAddins
        => Provider.GetRequiredService<ActivateAddinsViewModel>();

    /// <summary> Провайдер сервисов </summary>
    private static IServiceProvider Provider => Program.Provider;
}
