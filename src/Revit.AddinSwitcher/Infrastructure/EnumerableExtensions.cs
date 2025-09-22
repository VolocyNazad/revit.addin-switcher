using System.Collections.ObjectModel;

namespace Revit.AddinSwitcher.Infrastructure;
internal static class EnumerableExtensions
{
    public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> source) => new(source);
}
      
