namespace Revit.AddinSwitcher.Abstractions.ViewModels;

internal interface IAddinPathContainerViewModel
{
    IEnumerable<AddinInfoViewModel> Collection { get; set; }
}
