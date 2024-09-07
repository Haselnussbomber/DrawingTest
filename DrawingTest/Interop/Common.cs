using System;
using System.Collections.Generic;
using HaselCommon.Extensions;
using R3;

namespace DrawingTest.Interop;

public class Common
{
    private bool IsDisposed;
    private int IntervalId = 0;
    private int TimeoutId = 0;
    private readonly Dictionary<int, IDisposable> Intervals = [];
    private readonly Dictionary<int, IDisposable> Timeouts = [];

    internal void Dispose()
    {
        if (IsDisposed)
            return;

        Intervals.Dispose();
        IntervalId = 0;
        Timeouts.Dispose();
        TimeoutId = 0;

        IsDisposed = true;
    }

    public int setInterval(dynamic fn, double delay)
    {
        if (IsDisposed)
            return -1;

        var intervalId = IntervalId++;
        while (Timeouts.ContainsKey(intervalId))
            intervalId = IntervalId++;
        var subscription = Observable
            .Interval(TimeSpan.FromMilliseconds(delay))
            .Subscribe((unit) => fn());
        Intervals.Add(intervalId, subscription);
        return intervalId;
    }

    public void clearInterval(int id)
    {
        if (IsDisposed)
            return;

        if (Intervals.Remove(id, out var disposable))
            disposable.Dispose();
    }

    public int setTimeout(dynamic fn, double delay)
    {
        if (IsDisposed)
            return -1;

        var timerId = TimeoutId++;
        while (Timeouts.ContainsKey(timerId))
            timerId = TimeoutId++;
        var subscription = Observable
            .Timer(TimeSpan.FromMilliseconds(delay))
            .Subscribe((unit) =>
            {
                fn();
                clearTimeout(timerId);
            });
        Timeouts.Add(timerId, subscription);
        return timerId;
    }

    public void clearTimeout(int id)
    {
        if (IsDisposed)
            return;

        if (Timeouts.Remove(id, out var disposable))
            disposable.Dispose();
    }
}
