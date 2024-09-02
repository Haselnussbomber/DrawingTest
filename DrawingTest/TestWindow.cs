using System;
using FFXIVClientStructs.FFXIV.Client.UI;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Events;
using HaselCommon.Services;
using Microsoft.Extensions.Logging;

namespace DrawingTest;

public class TestWindow : Window
{
    public TestWindow(WindowManager WindowManager, ILogger<TestWindow> logger) : base(WindowManager, "TestWindow")
    {
        var type = GetType();

        var loader = new DocumentLoader { Logger = logger };
        loader.ElementRegistry.Add<ClockElement>("clock");

        Document = loader.FromManifestResource($"{type.Namespace}.{type.Name}.xml");
        Document.AddEventListener(OnEvent);

        var sestringtestNode = Document.GetNodeById("sestringtest");
        if (sestringtestNode != null)
        {
            //sestringtestNode.FirstChild?.LastChild?.
            //    sestringtestTextNode.Text = "Test String";
        }
    }

    public unsafe void OnEvent(Event evt)
    {
        switch (evt)
        {
            case MouseEvent mouseEvent:
                switch (mouseEvent.EventType)
                {
                    case MouseEventType.MouseClick:
                        switch (evt.Sender?.Id)
                        {
                            case "character-icon":
                                UIModule.Instance()->ExecuteMainCommand(2);
                                break;
                        }
                        break;

                    default:
                        Document?.Logger?.LogTrace("Unhandled {eventType} from {nodeDisplayName}!", Enum.GetName(mouseEvent.EventType), evt.Sender?.DisplayName ?? "unknown node");
                        break;
                }
                break;
        }
    }
}
