using CommunityToolkit.Mvvm.ComponentModel;

namespace Revit.AddinSwitcher.ViewModels.Base;

internal partial class SelectableViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isSelected = false;

    public object? Item { get; init; }

    public override bool Equals(object? obj) => obj is SelectableViewModel model 
        && EqualityComparer<object>.Default.Equals(Item, model.Item);

    public override int GetHashCode() => HashCode.Combine(Item);

    public override string ToString() => Item?.ToString() ?? string.Empty;

}

internal sealed partial class SelectableViewModel<T> 
    : SelectableViewModel where T : class
{
    public new T? Item { get => base.Item as T; init => base.Item = value; }

    public override bool Equals(object? obj) 
        => obj is SelectableViewModel<T> model 
        && base.Equals(obj) 
        && EqualityComparer<object?>.Default.Equals(Item, model.Item);

    public override int GetHashCode() 
        => HashCode.Combine(base.GetHashCode(), Item);

    public override string ToString() => Item?.ToString() ?? string.Empty;
}
