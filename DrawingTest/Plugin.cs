using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using HaselCommon;
using HaselCommon.Services;
using Microsoft.Extensions.DependencyInjection;
using R3;

namespace DrawingTest;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog PluginLog { get; private set; } = null!;

    public unsafe Plugin()
    {
        Service.Initialize(PluginInterface, PluginLog)
            .AddSingleton<FrameProvider, FrameworkFrameProvider>()
            .AddSingleton<GearsetRepository>()
            .AddSingleton<GearsetWindow>()
            .AddSingleton<TestWindow>();
        Service.BuildProvider();
        ObservableSystem.RegisterServiceProvider(Service.Provider);
        ObservableSystem.RegisterUnhandledExceptionHandler((ex) => PluginLog.Error(ex, "Unhandled Exception in ObservableSystem"));
        Service.Get<YogaLoggerService>();
        Service.Get<GearsetWindow>().Open();
        //Service.Get<TestWindow>().Open();
    }

    public void Dispose()
    {
        Service.Dispose();
    }
}
