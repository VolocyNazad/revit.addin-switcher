using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Revit.AddinSwitcher.Abstractions.ViewModels;
using Revit.AddinSwitcher.Infrastructure;
using Revit.AddinSwitcher.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Revit.AddinSwitcher.ViewModels;

[AutoConstructor(addParameterless: true)]
internal sealed partial class DeactivatedAddinPathContainerViewModel : InitializableObservableObject
{
    private readonly AddinPathContainerViewModel _ownerContainer;
    private readonly ILogger<DeactivatedAddinPathContainerViewModel> _logger;

    [AutoConstructorInitializer]
    private void OnInitialing() => RefreshCollectionView();

    [ObservableProperty]
    private string _searchField = string.Empty;

    public ObservableCollection<AddinInfoViewModel> SelectedItems
        => CollectionViewSource is not null
        ? ((ObservableCollection<SelectableViewModel<AddinInfoViewModel>>)CollectionViewSource.Source)
            .Where(i => i.Item is not null && IsTarget(i.Item))
            .Where(i => i.IsSelected).Select(i => i.Item)
            .Where(i => i is not null)
            .ToObservable()
        : [];  

    public CollectionViewSource? CollectionViewSource { get; private set; }

    protected override void OnInitializing()
    {
        _ownerContainer.PropertyChanged += AddinPathContainerViewModel_PropertyChanged;
        PropertyChanged += ActivatedAddinPathContainerViewModel_PropertyChanged;
    }


    protected override void OnDeinitializing()
    {
        _ownerContainer.PropertyChanged -= AddinPathContainerViewModel_PropertyChanged;
        PropertyChanged -= ActivatedAddinPathContainerViewModel_PropertyChanged;
    }

    private void ActivatedAddinPathContainerViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchField))
            RefreshCollectionView();
    }
    private void AddinPathContainerViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(_ownerContainer.Collection)) RefreshCollectionView();
    }
    private void RefreshCollectionView()
    {
        CollectionViewSource = new()
        {
            Source = _ownerContainer.Collection
                .Select(i => new SelectableViewModel<AddinInfoViewModel> { Item = i })
                .ToObservable(),
        };

        CollectionViewSource.Filter += CollectionViewSource_Filter;

        //CollectionViewSource.GroupDescriptions.Clear();
        //CollectionViewSource.GroupDescriptions.Add(
        //    new PropertyGroupDescription(nameof(AddinInfoViewModel.DirectoryPath)));

        //CollectionViewSource.SortDescriptions.Clear();
        //CollectionViewSource.SortDescriptions.Add(
        //    new SortDescription(nameof(AddinInfoViewModel.Path), ListSortDirection.Ascending));

        OnPropertyChanged(nameof(CollectionViewSource));

        _logger.LogDebug("Refreshed.");
    }

    private void CollectionViewSource_Filter(object sender, FilterEventArgs args)
    {
        args.Accepted = args.Item is SelectableViewModel<AddinInfoViewModel> viewModel
            && viewModel.Item is not null && IsTarget(viewModel.Item) && viewModel.Item.Path.Contains(SearchField ?? string.Empty);
    }
    private static bool IsTarget(AddinInfoViewModel item) => item.IsActive is false;
}
