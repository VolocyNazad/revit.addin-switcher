using Microsoft.Extensions.Logging;
using Revit.AddinSwitcher.Abstractions.Services;
using Revit.AddinSwitcher.Resources;
using System.IO;

namespace Revit.AddinSwitcher.Services;

[AutoConstructor]
internal sealed partial class ActivateAddinService : IActivateAddinService
{
    private readonly ILogger<ActivateAddinService> _logger;

    public async Task Deactivate(string addinManifestPath)
    {
        string sourcePath = addinManifestPath;

        string? sourceDirectoryPath = Path.GetDirectoryName(sourcePath);
        if (sourceDirectoryPath is null) return;

        string destinationDirectoryPath = Path.Combine(
            sourceDirectoryPath,
            Shared.DisabledAddinDirectoryName);
        if (!Directory.Exists(destinationDirectoryPath))
            Directory.CreateDirectory(destinationDirectoryPath);

        string ? destinationPath = Path.Combine(
            destinationDirectoryPath, 
            Path.GetFileName(sourcePath));
        if (destinationPath == null) return; // todo Обработать!

        using (FileStream sourceStream = File.Open(sourcePath, FileMode.Open))
        {
            using (FileStream destinationStream = File.Create(destinationPath))
            {
                await sourceStream.CopyToAsync(destinationStream);
            }
        }

        File.Delete(sourcePath);
    }

    public async Task Activate(string addinManifestPath)
    {
        string sourcePath = addinManifestPath;

        string? sourceDirectoryPath = Path.GetDirectoryName(sourcePath);
        if (sourceDirectoryPath is null) return;
        string? destinationDirectoryPath = Path.GetDirectoryName(sourceDirectoryPath);
        if (destinationDirectoryPath is null) return;

        string? destinationPath = Path.Combine(
            destinationDirectoryPath,
            Path.GetFileName(sourcePath));
        if (destinationPath == null) return; // todo Обработать!

        using (FileStream sourceStream = File.Open(sourcePath, FileMode.Open))
        {
            using (FileStream destinationStream = File.Create(destinationPath))
            {
                await sourceStream.CopyToAsync(destinationStream);
            }
        }

        File.Delete(sourcePath);
    }
}
