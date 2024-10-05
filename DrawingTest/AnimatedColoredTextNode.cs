using FFXIVClientStructs.FFXIV.Client.System.Framework;
using HaselCommon.Graphics;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Attributes;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Math;
using ImGuiNET;
using Lumina.Text.ReadOnly;

namespace DrawingTest;

public class AnimatedColoredTextNode : TextNode
{
    [NodeProp("Animation")]
    public float AnimationTimestamp { get; set; }

    [NodeProp("Animation")]
    public float AnimationDirection { get; set; } = 1f;

    private float _lastValue;

    public AnimatedColoredTextNode() : base()
    {
        TextColor = hsl(10, 0.80f, 0.75f);
    }

    public override unsafe void UpdateContent()
    {
        if (Display == Display.None)
            return;

        AnimationTimestamp += MathUtils.Clamp01(Framework.Instance()->FrameDeltaTime / 2f) * AnimationDirection;
        AnimationTimestamp = MathUtils.Clamp01(AnimationTimestamp);
        if (AnimationTimestamp is 0 or 1)
            AnimationDirection = -AnimationDirection;

        var value = -(MathF.Cos(MathF.PI * AnimationTimestamp) - 1f) / 2f; // InOutSine
        value *= 20f;

        if (value == _lastValue)
            return;

        _lastValue = value;

        PaddingTop = value;
        PaddingLeft = value / 2f;
        BorderLeft = value * 2f;

        var color = TextColor ?? Color.From(ImGuiCol.Text);
        Text = ReadOnlySeString.FromMacroString($"<color({(uint)color})>R: {color.R:0.00} G: {color.G:0.00} B: {color.B:0.00}<color(0)>");
    }
}
