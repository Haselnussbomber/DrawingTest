using System;
using FFXIVClientStructs.FFXIV.Client.UI;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Events;
using HaselCommon.Services;
using ImGuiNET;
using Microsoft.Extensions.Logging;

namespace DrawingTest;

public class TestWindow : Window
{
    public TestWindow(WindowManager WindowManager, ILogger<TestWindow> logger) : base(WindowManager, "TestWindow")
    {
        var type = GetType();

        Document = new Document
        {
            Logger = logger,
        };

        Document.CustomElements.Add<ClockNode>("clock");
        Document.AddEventListener(OnEvent);
        Document.LoadManifestResource($"{type.Namespace}.{type.Name}.xml");

        Flags |= ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
    }

    public override void Draw()
    {
        if (IsFocused && !Document!.ClassList.Contains("win-focused"))
            Document.ClassList.Add("win-focused");
        else if (!IsFocused && Document!.ClassList.Contains("win-focused"))
            Document.ClassList.Remove("win-focused");

        base.Draw();
    }

    public unsafe void OnEvent(Event evt)
    {
        switch (evt)
        {
            case MouseEvent mouseEvent:
                switch (mouseEvent.EventType)
                {
                    case MouseEventType.MouseClick:
                        if (mouseEvent.Button == ImGuiMouseButton.Left)
                        {
                            switch (evt.Sender?.Id)
                            {
                                case "character-icon":
                                    UIModule.Instance()->ExecuteMainCommand(2);
                                    break;
                                case "close-button":
                                    Close();
                                    break;
                            }
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
