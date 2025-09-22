using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Revit.AddinSwitcher.Abstractions.ViewModels;
using System.Collections.ObjectModel;

namespace Revit.AddinSwitcher.ViewModels;

internal sealed partial class AddinPathContainerViewModel : IAddinPathContainerViewModel
{
    IEnumerable<AddinInfoViewModel> IAddinPathContainerViewModel.Collection
    {
        get => Collection;
        set => Collection = new(value);
    }
}

[AutoConstructor(addParameterless: true)]
internal sealed partial class AddinPathContainerViewModel : ObservableObject
{
    private readonly ILogger<AddinPathContainerViewModel> _logger;

    [ObservableProperty]
    private ObservableCollection<AddinInfoViewModel> _collection = [];
}
