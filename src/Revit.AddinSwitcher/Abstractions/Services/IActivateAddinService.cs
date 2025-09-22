namespace Revit.AddinSwitcher.Abstractions.Services;

internal interface IActivateAddinService
{
    Task Activate(string addinManifestPath);

    Task Deactivate(string addinManifestPath);
}
