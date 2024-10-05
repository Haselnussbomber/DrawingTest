using System.Numerics;
using Dalamud.Interface;
using HaselCommon.Graphics;
using HaselCommon.Gui;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Attributes;
using HaselCommon.Gui.Yoga.Enums;
using ImGuiNET;

namespace DrawingTest;

public class FontAwesomeIconNode : Node
{
    private bool _hovered;

    [NodeProp("FontAweSomeIcon", editable: true)]
    public FontAwesomeIcon Icon { get; set; } = FontAwesomeIcon.ExclamationTriangle;

    [NodeProp("FontAweSomeIcon", editable: true)]
    public Color? IconDefaultColor { get; set; }

    [NodeProp("FontAweSomeIcon", editable: true)]
    public Color? IconHoveredColor { get; set; }

    public Action<FontAwesomeIconNode>? OnMouseOverCallback { get; set; }
    public Action<FontAwesomeIconNode>? OnMouseEnterCallback { get; set; }
    public Action<FontAwesomeIconNode>? OnMouseOutCallback { get; set; }
    public Action<FontAwesomeIconNode>? OnMouseClickCallback { get; set; }

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
                OnMouseEnterCallback?.Invoke(this);
            }
            else
            {
                OnMouseOutCallback?.Invoke(this);
            }
        }

        if (hovered)
        {
            OnMouseOverCallback?.Invoke(this);
        }

        if (ImGui.IsItemClicked())
        {
            OnMouseClickCallback?.Invoke(this);
        }
    }
}
