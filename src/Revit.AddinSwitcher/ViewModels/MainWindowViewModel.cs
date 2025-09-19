using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Revit.AddinSwitcher.ViewModels;

[AutoConstructor(addParameterless: true)]
internal sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IStringLocalizer<MainWindowViewModel> _localizer;

    public IStringLocalizer<MainWindowViewModel> Localizer => _localizer;
}
