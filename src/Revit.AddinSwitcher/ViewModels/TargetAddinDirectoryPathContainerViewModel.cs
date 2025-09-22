using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Revit.AddinSwitcher.Abstractions.ViewModels;
using System.Collections.ObjectModel;
using System.IO;

namespace Revit.AddinSwitcher.ViewModels;

[AutoConstructor(addParameterless: true)]
internal sealed partial class TargetAddinDirectoryPathContainerViewModel : ITargetAddinDirectoryPathContainerViewModel
{
    IEnumerable<DirectoryInfoViewModel> ITargetAddinDirectoryPathContainerViewModel.Collection 
    { 
        get => Collection; 
        set => Collection = new(value); 
    }
}
internal sealed partial class TargetAddinDirectoryPathContainerViewModel : InitializableObservableObject
{
    private readonly ILogger<AddinPathContainerViewModel> _logger;

    private readonly string _appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private readonly string _commonAppPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

    [AutoConstructorInitializer]
    private void OnInitialing() => Collection = [
            new(Path.Combine(_appPath, @$"Autodesk\Revit\Addins\2021")),
            new(Path.Combine(_commonAppPath, @$"Autodesk\Revit\Addins\2021"))
        ];


    [ObservableProperty]
    private ObservableCollection<DirectoryInfoViewModel> _collection = [];

}
