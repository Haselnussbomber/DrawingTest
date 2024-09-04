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
