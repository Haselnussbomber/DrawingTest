using System;
using System.IO;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using DrawingTest.Interop;
using HaselCommon;
using HaselCommon.ImGuiYoga;
using HaselCommon.Services;
using Microsoft.ClearScript.V8;
using R3;

namespace DrawingTest;

public sealed class Plugin : IDalamudPlugin
{
    private readonly CompositeDisposable Disposables = [];
    private V8ScriptEngine? Engine;
    private Window? Window;
    private Common? Common;
    private readonly string ScriptPath;
    private DateTime LastFileWrite = DateTime.MinValue;

    public IDalamudPluginInterface PluginInterface { get; }
    public IPluginLog PluginLog { get; }

    public unsafe Plugin(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog)
    {
        PluginInterface = pluginInterface;
        PluginLog = pluginLog;

        Service.Initialize(pluginInterface, pluginLog);

        ScriptPath = Path.Join(pluginInterface.AssemblyLocation.Directory!.FullName, @"..\..\..\..\drawing-test\dist\main.js");
        if (!new FileInfo(ScriptPath).Exists)
            throw new FileNotFoundException("Script not found.", ScriptPath);

        CheckFile(new Unit());

        Observable
            .Interval(TimeSpan.FromSeconds(2))
            .Subscribe(CheckFile)
            .AddTo(Disposables);

        PluginInterface.UiBuilder.OpenMainUi += UiBuilder_OpenMainUi;
    }

    private void CheckFile(Unit unit)
    {
        var fileWriteTime = new FileInfo(ScriptPath).LastWriteTimeUtc;
        if (fileWriteTime > LastFileWrite)
        {
            LastFileWrite = fileWriteTime;
            PluginLog.Debug("Script changed, restarting...");
            StartEngine();
        }
    }

    public void Dispose()
    {
        PluginInterface.UiBuilder.OpenMainUi -= UiBuilder_OpenMainUi;
        Window?.Dispose();
        Window = null;
        Disposables.Dispose();
        StopEngine();
        Service.Dispose();
    }

    private void StartEngine()
    {
        StopEngine();

        Window = new Window(Service.Get<WindowManager>(), "Test Window");

        Common = new Common();
        Engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging);
        Engine.AddHostType("console", typeof(Interop.Console));
        Engine.AddHostType("EventSource", typeof(EventSource));
        Engine.AddHostObject("Common", Common);
        Engine.AddHostObject("document", Window.Document);

        try
        {
            Engine.Evaluate(File.ReadAllText(ScriptPath));
        }
        catch (Exception e)
        {
            PluginLog.Error(e, "Unexpected script error");
        }
    }

    private void StopEngine()
    {
        Common?.Dispose();
        Window?.OnClose();
        Window?.Dispose();
        Engine?.Dispose();
    }

    private void UiBuilder_OpenMainUi()
    {
        Window?.Toggle();
    }
}
