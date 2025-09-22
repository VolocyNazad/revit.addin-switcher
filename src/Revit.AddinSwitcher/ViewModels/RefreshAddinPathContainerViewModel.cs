using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Revit.AddinSwitcher.Abstractions.Services;
using Revit.AddinSwitcher.Abstractions.ViewModels;
using Revit.AddinSwitcher.Resources;
using System.IO;

namespace Revit.AddinSwitcher.ViewModels;

[AutoConstructor(addParameterless: true)]
internal sealed partial class RefreshAddinPathContainerViewModel : InitializableObservableObject
{
    private readonly IAddinPathContainerViewModel _container;
    private readonly ITargetAddinDirectoryPathContainerViewModel _directoryContainer;
    private readonly IMemoryCache _cache;
    private readonly IDebounceDispatcher _debounceDispatcher;
    private readonly ILogger<RefreshAddinPathContainerViewModel> _logger;
    private readonly IStringLocalizer<RefreshAddinPathContainerViewModel> _localizer;


    private const string _addinExtension = "*.addin";

    public IStringLocalizer<RefreshAddinPathContainerViewModel> Localizer => _localizer;

    #region [Refresh] Command - Обновить список информации о плагинах 

    /// <summary> Обновить список информации о плагинах </summary>
    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private void Refresh()
    {
        RefreshInfoAboutAddins();
        foreach (var directoryInfo in _directoryContainer.Collection) {
            StopWatching(directoryInfo.Path);
            RunWatching(directoryInfo.Path);
        }
    }

    private bool CanRefresh() => _directoryContainer.Collection.Any();

    #endregion

    protected override void OnInitializing()
    {
        RefreshInfoAboutAddins();
        foreach (var directoryInfo in _directoryContainer.Collection) 
            RunWatching(directoryInfo.Path);
    }
    protected override void OnDeinitializing()
    {
        foreach (var directoryInfo in _directoryContainer.Collection)
            StopWatching(directoryInfo.Path);
    }

    private void StopWatching(string path)
    {
        bool started = _cache.TryGetValue<FileSystemWatcher>(path, out var watcher);
        if (!started)
            throw new InvalidOperationException("Failed to stop directory watch. Directory not started!");
        _cache.Remove(path);
        watcher?.Dispose();
    }
    private void RunWatching(string path)
    {
        bool started = _cache.TryGetValue<FileSystemWatcher>(path, out var watcher);
        if (started)
            throw new InvalidOperationException("Failed to start directory watch. Directory already started!");

        if (!Directory.Exists(path))
            throw new InvalidOperationException("Failed to start directory watch. Directory not exists!");

        watcher = new()
        {
            Path = path,
            IncludeSubdirectories = true,
            Filter = "*.*",
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName
                         | NotifyFilters.Size | NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        watcher.Created += OnFileEvent;
        watcher.Deleted += OnFileEvent;
        watcher.Changed += OnFileEvent;
        watcher.Renamed += OnRenamed;
        watcher.Error += OnError;

        _cache.Set(path, watcher);
    }
    private void OnFileEvent(object sender, FileSystemEventArgs args) 
        => _debounceDispatcher.Debounce(2000, args => RefreshInfoAboutAddins(), args);
    private void OnRenamed(object sender, RenamedEventArgs args)
        => _debounceDispatcher.Debounce(2000, args => RefreshInfoAboutAddins(), args);
    private void OnError(object sender, ErrorEventArgs args)
        => _debounceDispatcher.Debounce(2000, args => RefreshInfoAboutAddins(), args);

    private void RefreshInfoAboutAddins()
    {
        IEnumerable<string> directories = _directoryContainer.Collection
              .Select(i => i.Path)
              .ToList();

        HashSet<string> uniqueDirectories = [.. directories];
        if (directories.Count() != uniqueDirectories.Count)
        {
            _logger.LogInformation("Two duplicate paths were selected for searching addin manifests. ");
        }

        List<AddinInfoViewModel> result = [];
        foreach (string folder in uniqueDirectories)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                _logger.LogWarning("Empty path found in folder list. ");
                continue;
            }

            string normalizedDirectory = Path.GetFullPath(folder);

            if (!Directory.Exists(normalizedDirectory))
            {
                _logger.LogWarning("Folder does not exist: {folder}", normalizedDirectory);
                continue;
            }

            try
            {
                _logger.LogInformation("Searching for .addin files in folder: {folder}", normalizedDirectory);
                ICollection<string> files = Directory.GetFiles(normalizedDirectory, _addinExtension);

                _logger.LogDebug("Found {Count} files in {folder}", files.Count, normalizedDirectory);
                result.AddRange(files.Select(path => new AddinInfoViewModel(path, true)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get list of files {folder}", normalizedDirectory);
            }

            string disabledAddinDirectory = Path.Combine(normalizedDirectory, Shared.DisabledAddinDirectoryName);

            try
            {
                _logger.LogInformation("Searching for .addin files in folder: {folder}", disabledAddinDirectory);
                ICollection<string> files = Directory.GetFiles(disabledAddinDirectory, _addinExtension);

                _logger.LogDebug("Found {Count} files in {folder}", files.Count, disabledAddinDirectory);
                result.AddRange(files.Select(path => new AddinInfoViewModel(path, false)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get list of files {folder}", disabledAddinDirectory);
            }
        }

        _container.Collection = result;
    }
}

