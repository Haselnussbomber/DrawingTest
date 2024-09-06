using System;
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

        Document.CustomElements.Add<ClockElement>("clock");
        Document.AddEventListener(OnEvent);
        Document.LoadManifestResource($"{type.Namespace}.{type.Name}.xml");

        Flags |= ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;

#if false
        Document.QuerySelectorAll(".wrapper > #character-icon").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".selectortest > .first-child").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".selectortest > .child").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".selectortest .child ~ .child").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child[id]").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child[id=\"first-child-child2withid\"]").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child[id~=\"first-child-child2withid\"]").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child[id^=\"first-child\"]").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child[id$=\"child2withid\"]").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child[id*=\"child2\"]").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child:nth-child(2)").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child:nth-child(odd)").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child:nth-child(even)").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child:first-child").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child:last-child").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child:has(.superchild)").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#elif false
        Document.QuerySelectorAll(".child:not(.insane)").ForEach(node =>
        {
            node.Style.Border = 1;
            node.Style.BorderColor = new HaselColor(0, 1, 0);
        });
#endif
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
                        switch (evt.Sender?.Id)
                        {
                            case "character-icon":
                                //UIModule.Instance()->ExecuteMainCommand(2);

                                var red = Document?.QuerySelector(".red");
                                if (red != null)
                                {
                                    red.Attributes["style"] = "border-color: #0F0";
                                }
                                break;
                            case "close-button":
                                Close();
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
