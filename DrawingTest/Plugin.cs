using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using HaselCommon;
using Microsoft.Extensions.DependencyInjection;

namespace DrawingTest;

public sealed class Plugin : IDalamudPlugin
{
    public unsafe Plugin(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog)
    {
        Service.Initialize(pluginInterface, pluginLog)
            .AddSingleton<TestWindow>();

        Service.BuildProvider();

        Service.Get<TestWindow>().Open();
    }

    public void Dispose()
    {
        Service.Dispose();
    }
}
