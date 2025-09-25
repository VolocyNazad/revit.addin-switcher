using CommunityToolkit.Mvvm.ComponentModel;
using Revit.AddinSwitcher.Abstractions.ViewModels;

namespace Revit.AddinSwitcher.ViewModels;

internal sealed partial class AddinInfoVersionFilterViewModel : ObservableObject
{
    public AddinInfoVersionFilterViewModel(bool isActive, string value)
    {
        IsActive = isActive;
        Value = value;
    }

    [ObservableProperty]
    public bool _isActive;

    public string Value { get; init; }

    public bool IsValid(AddinInfoViewModel item) => item.Version == Value;


    public override string? ToString() => Value?.ToString();
}
