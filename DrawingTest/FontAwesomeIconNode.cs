using System.Numerics;
using Dalamud.Interface;
using HaselCommon.Graphics;
using HaselCommon.Gui;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Attributes;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Gui.Yoga.Events;
using ImGuiNET;

namespace DrawingTest;

public class FontAwesomeIconNode : Node
{
    private bool _hovered;
    private Vector2 _mousePos;

    [NodeProp("FontAwesomeIcon", editable: true)]
    public FontAwesomeIcon Icon { get; set; } = FontAwesomeIcon.ExclamationTriangle;

    [NodeProp("FontAwesomeIcon", editable: true)]
    public Color? IconDefaultColor { get; set; }

    [NodeProp("FontAwesomeIcon", editable: true)]
    public Color? IconHoveredColor { get; set; }

    public FontAwesomeIconNode()
    {
        NodeType = NodeType.Text;
        HasMeasureFunc = true;
    }

    public override Vector2 Measure(float width, MeasureMode widthMode, float height, MeasureMode heightMode)
    {
        return ImGuiUtils.GetIconSize(Icon);
    }

    public override void DrawContent()
    {
        ImGuiUtils.Icon(Icon, _hovered ? (IconHoveredColor ?? IconDefaultColor) : IconDefaultColor);

        var hovered = ImGui.IsItemHovered();
        if (hovered != _hovered)
        {
            _hovered = hovered;

            if (hovered)
            {
                DispatchEvent(new MouseEvent()
                {
                    EventType = MouseEventType.MouseOver,
                });
            }
            else
            {
                DispatchEvent(new MouseEvent()
                {
                    EventType = MouseEventType.MouseOut
                });
            }
        }

        if (hovered)
        {
            var mousePos = ImGui.GetMousePos();
            if (mousePos != _mousePos)
            {
                _mousePos = mousePos;
                DispatchEvent(new MouseEvent()
                {
                    EventType = MouseEventType.MouseMove
                });
            }

            DispatchEvent(new MouseEvent()
            {
                EventType = MouseEventType.MouseHover
            });
        }

        if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
        {
            DispatchEvent(new MouseEvent()
            {
                EventType = MouseEventType.MouseClick,
                Button = ImGuiMouseButton.Left,
            });
        }
        else if (ImGui.IsItemClicked(ImGuiMouseButton.Middle))
        {
            DispatchEvent(new MouseEvent()
            {
                EventType = MouseEventType.MouseClick,
                Button = ImGuiMouseButton.Middle,
            });
        }
        else if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
        {
            DispatchEvent(new MouseEvent()
            {
                EventType = MouseEventType.MouseClick,
                Button = ImGuiMouseButton.Right,
            });
        }
    }
}
