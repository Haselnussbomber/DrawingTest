using HaselCommon.Services;
using HaselCommon.Windowing;
using ImGuiNET;
using R3;

namespace DrawingTest;

internal class TestWindow(WindowManager wm) : SimpleWindow(wm, "TestWindow")
{
    private string text = string.Empty;
    private int frameCount = 0;
    private CompositeDisposable? disposable = null;

    public override void OnOpen()
    {
        RegisterObservables();
        base.OnOpen();
    }

    public override void OnClose()
    {
        UnregisterObservables();
        base.OnClose();
    }

    public override void Dispose()
    {
        UnregisterObservables();
        base.Dispose();
    }

    public override void Draw()
    {
        ImGui.TextUnformatted(text);
    }

    private void RegisterObservables()
    {
        disposable = [];

        Observable
            .EveryUpdate()
            .SkipWhile((_, x) => !IsOpen)
            .Subscribe(x =>
            {
                text = $"Frame: {frameCount++}";
            })
        .AddTo(disposable);
    }

    private void UnregisterObservables()
    {
        disposable?.Dispose();
        disposable = null;
    }
}
