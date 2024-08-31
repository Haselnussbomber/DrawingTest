using System;
using FFXIVClientStructs.FFXIV.Client.UI;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Events;
using HaselCommon.Services;
using Microsoft.Extensions.Logging;

namespace DrawingTest;

public class TestWindow : YogaWindow
{
    public TestWindow(WindowManager WindowManager, ILogger<TestWindow> logger) : base(WindowManager, "TestWindow")
    {
        Context.Logger = logger;
        Context.RegisterType<ClockNode>();
        Context.LoadResources();
    }

    public override unsafe void OnEvent(YogaEvent evt)
    {
        switch (evt)
        {
            case YogaMouseEvent mouseEvent:
                switch (mouseEvent.EventType)
                {
                    case YogaMouseEventType.MouseClick:
                        switch (evt.Sender?.Id)
                        {
                            case "character-icon":
                                UIModule.Instance()->ExecuteMainCommand(2);
                                break;
                        }
                        break;

                    default:
                        Context.Logger?.LogTrace("Unhandled {eventType} from {nodeDisplayName}!", Enum.GetName(mouseEvent.EventType), evt.Sender?.DisplayName ?? "unknown node");
                        break;
                }
                break;
        }
    }
}
