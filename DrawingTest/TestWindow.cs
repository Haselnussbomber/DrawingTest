using FFXIVClientStructs.FFXIV.Client.UI;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Core;
using HaselCommon.Services;
using Microsoft.Extensions.Logging;

namespace DrawingTest;

public class TestWindow : YogaWindow
{
    public TestWindow(WindowManager WindowManager, ILogger<TestWindow> logger) : base(WindowManager, "TestWindow")
    {
        Context.Logger = logger;
        Context.RegisterType<ClockNode>();
        RootNode = YogaLoader.LoadManifestResource(Context, "DrawingTest.TestWindow.xml");

        var characterIcon = RootNode?.GetNodeById("character-icon");
        if (characterIcon != null)
            characterIcon.Interactable = true;
        else
            Context.Logger?.LogError("Node character-icon not found!");
    }

    public override bool OnEvent(YogaEventType eventType, YogaNode? sender, params object[] args)
    {
        switch (eventType)
        {
            case YogaEventType.MouseClick when sender?.Id == "character-icon":
                unsafe { UIModule.Instance()->ExecuteMainCommand(2); }
                return true;

            case YogaEventType.MouseClick:
                Context.Logger?.LogTrace("Mouse click from {nodeDisplayName}!", sender?.DisplayName ?? "unknown node");
                return true;
        }

        return base.OnEvent(eventType, sender, args);
    }
}
