using System.IO;

namespace Revit.AddinSwitcher.Abstractions.ViewModels;

internal sealed record AddinInfoViewModel(string Path, bool IsActive)
{
    public string Name => System.IO.Path.GetFileNameWithoutExtension(Path) 
        ?? string.Empty;
    public string DirectoryPath => System.IO.Path.GetDirectoryName(Path) 
        ?? string.Empty;
    public string? Version => Directory.Exists(DirectoryPath) 
        ? new DirectoryInfo(DirectoryPath).Name 
        : string.Empty;
}
