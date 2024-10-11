using System.Numerics;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Attributes;
using HaselCommon.Gui.Yoga.Events;
using ImGuiNET;
using Lumina.Text.ReadOnly;
using YogaSharp;

namespace DrawingTest;

#pragma warning disable SeStringRenderer

public class ButtonNode : Node
{
    private readonly TextNode _textNode;
    private bool _hovered;
    private bool _active;
    private Vector2 _mousePos;

    [NodeProp("Button", editable: true)]
    public ReadOnlySeString Text
    {
        get => _textNode.Text;
        set => _textNode.Text = value;
    }

    public ButtonNode() : base()
    {
        PaddingHorizontal = 14;
        PaddingVertical = 7;
        AlignSelf = YGAlign.FlexStart;

        Add(_textNode = new());
    }

    public override void DrawContent()
    {
        DrawBackground();
        HandleInteraction();
    }

    private void DrawBackground()
    {
        var buttonColor = ImGuiCol.Button;
        if (_hovered) buttonColor = ImGuiCol.ButtonHovered;
        if (_active) buttonColor = ImGuiCol.ButtonActive;

        var pos = ImGui.GetWindowPos() + AbsolutePosition;
        ImGui.GetWindowDrawList().AddRectFilled(pos, pos + ComputedSize, ImGui.GetColorU32(buttonColor), 3);
    }

    private void HandleInteraction()
    {
        ImGui.SetCursorPos(AbsolutePosition + new Vector2(ComputedBorderLeft, ComputedBorderTop));

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
