using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using HaselCommon;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Components;
using HaselCommon.ImGuiYoga.Core;
using HaselCommon.Services;

namespace DrawingTest;

public class TestWindow : YogaWindow
{
    public TestWindow(WindowManager WindowManager) : base(WindowManager, "TestWindow")
    {
        Context.RegisterType<ClockNode>();
        RootNode = YogaLoader.LoadManifestResource(Context, "DrawingTest.TestWindow.xml");

        var characterIconNode = RootNode?.GetNodeById<IconNode>("character-icon");
        if (characterIconNode != null)
        {
            characterIconNode.OnClick = () => { unsafe { UIModule.Instance()->ExecuteMainCommand(2); } };
        }
        else
        {
            Service.Get<IPluginLog>().Warning("character-icon not found");
        }
    }
}
