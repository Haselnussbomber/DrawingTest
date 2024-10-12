using System.Numerics;
using Dalamud.Interface;
using HaselCommon.Graphics;
using HaselCommon.Gui;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Attributes;
using HaselCommon.Gui.Yoga.Events;
using ImGuiNET;
using YogaSharp;

namespace DrawingTest;

public class FontAwesomeIconNode : Node
{
    private bool _hovered;
    private Vector2 _mousePos;
    private bool _active;

    [NodeProp("FontAwesomeIcon", editable: true)]
    public FontAwesomeIcon Icon { get; set; } = FontAwesomeIcon.ExclamationTriangle;

    [NodeProp("FontAwesomeIcon", editable: true)]
    public Color? IconDefaultColor { get; set; }

    [NodeProp("FontAwesomeIcon", editable: true)]
    public Color? IconHoveredColor { get; set; }

    public FontAwesomeIconNode() : base()
    {
        NodeType = YGNodeType.Text;
        EnableMeasureFunc = true;
    }

    public override Vector2 Measure(float width, YGMeasureMode widthMode, float height, YGMeasureMode heightMode)
    {
        return ImGuiUtils.GetIconSize(Icon);
    }

    public override void DrawContent()
    {
        ImGuiUtils.Icon(Icon, _hovered ? (IconHoveredColor ?? IconDefaultColor) : IconDefaultColor);
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        ImGui.SetCursorPos(AbsolutePosition);

        if (ImGui.InvisibleButton($"###{Guid}_Button", ComputedSize))
        {
            DispatchEvent(new MouseEvent()
            {
                EventType = MouseEventType.MouseClick,
                Button = ImGuiMouseButton.Left
            });
        }

        var hovered = ImGui.IsItemHovered();
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

        var active = ImGui.IsItemActive();
        if (active != _active)
        {
            _active = active;

            if (active)
            {
                DispatchEvent(new MouseEvent()
                {
                    EventType = MouseEventType.MouseDown,
                });
            }
            else
            {
                DispatchEvent(new MouseEvent()
                {
                    EventType = MouseEventType.MouseUp
                });
            }
        }
    }
}
