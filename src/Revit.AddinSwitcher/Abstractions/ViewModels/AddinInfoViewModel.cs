using System.IO;

namespace Revit.AddinSwitcher.Abstractions.ViewModels;

internal sealed record AddinInfoViewModel(string Path, bool IsActive)
{
    public string Name => System.IO.Path.GetFileNameWithoutExtension(Path) 
        ?? string.Empty;
    public string DirectoryPath => System.IO.Path.GetDirectoryName(Path) 
        ?? string.Empty;
    public string? Version
    {
        get
        {
            if (Directory.Exists(DirectoryPath))
            {
                string? hostDirectoryName = new DirectoryInfo(DirectoryPath).Name;
                if (int.TryParse(hostDirectoryName, out _))
                    return hostDirectoryName;
                else
                {
                    hostDirectoryName = new DirectoryInfo(DirectoryPath).Parent?.Name;
                    if (int.TryParse(hostDirectoryName, out _))
                        return hostDirectoryName;
                }
                
            }
            return string.Empty;
        }
    }
}
