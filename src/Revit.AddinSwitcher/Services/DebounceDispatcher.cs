using Revit.AddinSwitcher.Abstractions.Services;
using System.Windows;
using System.Windows.Threading;

namespace Revit.AddinSwitcher.Services;

internal sealed class DebounceDispatcher : IDebounceDispatcher, IDisposable
{
    private readonly Lock _lock = new();
    private DispatcherTimer? timer;
    private DateTime lastExecution = DateTime.UtcNow.AddYears(-1);

    public void Debounce<TParameter>(int interval, Action<TParameter?> action,
        TParameter? parameter = default,
        DispatcherPriority priority = DispatcherPriority.Normal,
        Dispatcher? dispatcher = default)
    {
        using (_lock.EnterScope())
        {
            timer?.Stop();

            dispatcher ??= Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

            if (dispatcher.CheckAccess() && !dispatcher.HasShutdownStarted)
                return;

            timer = new(TimeSpan.FromMilliseconds(interval), priority, (s, e) =>
            {
                using (_lock.EnterScope())
                {
                    timer?.Stop();
                }
                action.Invoke(parameter);
            }, dispatcher);

            timer.Start();
        }
    }

    public void Throttle<TParameter>(int interval, Action<TParameter?> action,
        TParameter? parameter = default,
        DispatcherPriority priority = DispatcherPriority.Normal,
        Dispatcher? dispatcher = default)
    {
        using (_lock.EnterScope())
        {
            timer?.Stop();

            dispatcher ??= Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

            if (dispatcher.CheckAccess() && !dispatcher.HasShutdownStarted)
                return;

            var currentTime = DateTime.UtcNow;
            var timeSinceLast = currentTime - lastExecution;

            if (timeSinceLast.TotalMilliseconds < interval)
            {
                interval -= (int)timeSinceLast.TotalMilliseconds;
            }
            else
            {
                action.Invoke(parameter);
                lastExecution = currentTime;
                return;
            }

            timer = new(TimeSpan.FromMilliseconds(interval), priority, (s, e) =>
            {
                using (_lock.EnterScope())
                {
                    timer?.Stop();
                    lastExecution = DateTime.UtcNow;
                }
                action.Invoke(parameter);
            }, dispatcher);

            timer.Start();
        }
    }

    public void Dispose()
    {
        using (_lock.EnterScope())
        {
            timer?.Stop();
            timer = null;
        }
    }
}