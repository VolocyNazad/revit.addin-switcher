using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Revit.AddinSwitcher.Abstractions.Services;

namespace Revit.AddinSwitcher.ViewModels;

[AutoConstructor(addParameterless: true)]
internal sealed partial class ActivateAddinsViewModel : InitializableObservableObject
{
    private readonly ILogger<ActivateAddinsViewModel> _logger;
    private readonly IStringLocalizer<ActivateAddinsViewModel> _localizer;
    private readonly IActivateAddinService _activateAddinService;
    private readonly ActivatedAddinPathContainerViewModel _activatedAddinPathContainer;
    private readonly DeactivatedAddinPathContainerViewModel _deactivatedAddinPathContainer;

    public IStringLocalizer<ActivateAddinsViewModel> Localizer => _localizer;

    #region [ActivateAddins] Command - Активировать плагин 

    /// <summary> Активировать плагин </summary>
    [RelayCommand(CanExecute = nameof(CanActivateAddins))]
    private void ActivateAddins()
    {
        if (_deactivatedAddinPathContainer.SelectedItems is null) return;
        var collection = _deactivatedAddinPathContainer.SelectedItems
            .Select(i => i.Path).ToList();
        foreach (var item in collection)
        {
            _activateAddinService.Activate(item);
        }
    }

    private bool CanActivateAddins() => true;

    #endregion

    #region [DeactivateAddins] Command - Деактивировать плагин 

    /// <summary> Деактивировать плагин </summary>
    [RelayCommand(CanExecute = nameof(CanDeactivateAddins))]
    private void DeactivateAddins()
    {
        if (_activatedAddinPathContainer.SelectedItems is null) return;
        var collection = _activatedAddinPathContainer.SelectedItems
            .Select(i => i.Path).ToList();
        foreach (var item in collection)
        {
            _activateAddinService.Deactivate(item);
        }
    }

    private bool CanDeactivateAddins() => true;

    #endregion
}
