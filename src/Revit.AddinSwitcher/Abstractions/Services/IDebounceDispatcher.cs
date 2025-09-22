using System.Windows.Threading;

namespace Revit.AddinSwitcher.Abstractions.Services;

internal interface IDebounceDispatcher
{
    void Debounce<TParameter>(int interval, Action<TParameter?> action,
        TParameter? parameter = default,
        DispatcherPriority priority = DispatcherPriority.ApplicationIdle,
        Dispatcher? dispatcher = null);

    void Throttle<TParameter>(int interval, Action<TParameter?> action,
        TParameter? parameter = default,
        DispatcherPriority priority = DispatcherPriority.ApplicationIdle,
        Dispatcher? dispatcher = null);
}
