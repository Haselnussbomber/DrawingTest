using System;
using Dalamud.Plugin.Services;
using R3;
using R3.Collections;

namespace DrawingTest;

public class FrameworkFrameProvider : FrameProvider, IDisposable
{
    private readonly IFramework framework;
    private readonly object gate = new();
    private FreeListCore<IFrameRunnerWorkItem> list;
    private long frameCount;
    private bool isDisposed;

    public FrameworkFrameProvider(IFramework framework)
    {
        this.framework = framework;
        list = new FreeListCore<IFrameRunnerWorkItem>(gate);
        framework.Update += Framework_Update;
    }

    public void Dispose()
    {
        lock (gate)
        {
            framework.Update -= Framework_Update;
            list.Dispose();
            isDisposed = true;
        }
        GC.SuppressFinalize(this);
    }

    public override long GetFrameCount()
    {
        return frameCount;
    }

    public override void Register(IFrameRunnerWorkItem callback)
    {
        ObjectDisposedException.ThrowIf(isDisposed, this);
        list.Add(callback, out _);
    }

    private void Framework_Update(IFramework framework)
    {
        var span = list.AsSpan();
        for (var i = 0; i < span.Length; i++)
        {
            ref readonly var item = ref span[i];
            if (item != null)
            {
                try
                {
                    if (!item.MoveNext(frameCount))
                    {
                        list.Remove(i);
                    }
                }
                catch (Exception ex)
                {
                    list.Remove(i);
                    try
                    {
                        ObservableSystem.GetUnhandledExceptionHandler().Invoke(ex);
                    }
                    catch { }
                }
            }
        }
        frameCount++;
    }
}
