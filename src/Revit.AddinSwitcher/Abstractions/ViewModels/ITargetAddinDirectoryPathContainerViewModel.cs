namespace Revit.AddinSwitcher.Abstractions.ViewModels;

internal interface ITargetAddinDirectoryPathContainerViewModel
{
    IEnumerable<DirectoryInfoViewModel> Collection { get; set; }
}
