using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using HaselCommon;
using Microsoft.Extensions.DependencyInjection;

namespace DrawingTest;

public sealed class Plugin : IDalamudPlugin
{
    public IDalamudPluginInterface PluginInterface { get; }

    public unsafe Plugin(IDalamudPluginInterface pluginInterface, IPluginLog pluginLog)
    {
        PluginInterface = pluginInterface;

        Service.Initialize(pluginInterface, pluginLog)
            .AddSingleton<TestWindow>();

        Service.BuildProvider();

        Service.Get<TestWindow>().Open();

        pluginInterface.UiBuilder.OpenMainUi += UiBuilder_OpenMainUi;
    }

    public void Dispose()
    {
        PluginInterface.UiBuilder.OpenMainUi -= UiBuilder_OpenMainUi;
        Service.Dispose();
    }

    private void UiBuilder_OpenMainUi()
    {
        Service.Get<TestWindow>().Toggle();
    }
}
