using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Revit.AddinSwitcher.ViewModels;

internal abstract partial class InitializableObservableObject 
    : ObservableObject
{
    private bool initialized;

    #region [Initialize] Command - Инициализировать 

    /// <summary> Инициализировать </summary>
    [RelayCommand(CanExecute = nameof(CanInitialize))]
    private void Initialize()
    {
        OnInitializing();
        initialized = true;
    }

    private bool CanInitialize() => !initialized;

    #endregion

    #region [Deinitialize] Command - Деинициализировать 

    /// <summary> Деинициализировать </summary>
    [RelayCommand(CanExecute = nameof(CanDeinitialize))]
    private void Deinitialize()
    {
        OnDeinitializing();
        initialized = false;
    }

    private bool CanDeinitialize() => initialized;

    #endregion

    protected virtual void OnInitializing() { }
    protected virtual void OnDeinitializing() { }
}
