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

        var sestringtestNode = Document.QuerySelector(".wrapper > #character-icon");
        if (sestringtestNode != null)
        {
            logger.LogDebug("NODE FOUND! {displayName}", sestringtestNode.DisplayName);
            //sestringtestNode.FirstChild?.LastChild?.
            //    sestringtestTextNode.Text = "Test String";
        }
        else
        {
            logger.LogDebug("node not found");
        }

        logger.LogDebug("TEST 1 START");
        logger.LogDebug("TEST 1 RESULT: {displayName}", Document.QuerySelector(".selectortest > .first-child")?.DisplayName);
        logger.LogDebug("TEST 2 START");
        logger.LogDebug("TEST 2 RESULT: {displayName}", Document.QuerySelector(".selectortest > .child")?.DisplayName); // null
        logger.LogDebug("TEST 3 START");
        var firstChild = Document.QuerySelector(".selectortest .child");
        logger.LogDebug("TEST 3 RESULT: {displayName}", firstChild?.DisplayName);
        logger.LogDebug("TEST 3 NEXT: {displayName}", firstChild?.NextSibling?.DisplayName);
        logger.LogDebug("TEST 4 START");
        logger.LogDebug("TEST 4 RESULT: {displayName}", Document.QuerySelector(".selectortest .child + .child")?.DisplayName);

        foreach (var node in Document.QuerySelectorAll(".selectortest .child ~ .child"))
        {
            logger.LogDebug("node for selector {selector}: {displayName}", ".selectortest .child ~ .child", node.DisplayName);
        }

        logger.LogDebug("TEST 5 START");
        logger.LogDebug("TEST 5 RESULT: {displayName}", Document.QuerySelector(".child[id]")?.DisplayName);

        logger.LogDebug("TEST 6 START");
        logger.LogDebug("TEST 6 RESULT: {displayName}", Document.QuerySelector(".child[id=\"first-child-child2withid\"]")?.DisplayName);

        logger.LogDebug("TEST 7 START");
        logger.LogDebug("TEST 7 RESULT: {displayName}", Document.QuerySelector(".child[id~=\"first-child-child2withid\"]")?.DisplayName);

        logger.LogDebug("TEST 8 START");
        logger.LogDebug("TEST 8 RESULT: {displayName}", Document.QuerySelector(".child[id^=\"first-child\"]")?.DisplayName);

        logger.LogDebug("TEST 9 START");
        logger.LogDebug("TEST 9 RESULT: {displayName}", Document.QuerySelector(".child[id$=\"child2withid\"]")?.DisplayName);

        logger.LogDebug("TEST 10 START");
        logger.LogDebug("TEST 10 RESULT: {displayName}", Document.QuerySelector(".child[id*=\"child2\"]")?.DisplayName);

        // TO BE IMPLEMENTED
        logger.LogDebug("TEST 11 START");
        logger.LogDebug("TEST 11 RESULT: {displayName}", Document.QuerySelector(".child:nth-child(2)")?.DisplayName);

        logger.LogDebug("TEST 12 START");
        logger.LogDebug("TEST 12 RESULT: {displayName}", Document.QuerySelector(".child:nth-child(odd)")?.DisplayName);
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
