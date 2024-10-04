using FFXIVClientStructs.FFXIV.Client.System.Framework;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Math;
using Lumina.Text.ReadOnly;

namespace DrawingTest;

public class AnimatedColoredTextNode : TextNode
{
    private float _t;
    private float _direction = 1f;
    private float _lastValue;

    public override unsafe void UpdateContent()
    {
        if (Display == Display.None)
            return;

        _t += MathUtils.Clamp01(Framework.Instance()->FrameDeltaTime / 2f * _direction);
        if (_t == 0 || _t == 1)
            _direction = -_direction;

        var value = -(MathF.Cos(MathF.PI * _t) - 1f) / 2f; // InOutSine
        value *= 20f;

        if (value == _lastValue)
            return;

        _lastValue = value;

        PaddingTop = value;
        PaddingLeft = value / 2f;
        BorderLeft = value * 2f;

        var color = hsl(value * 10, 0.80f, 0.75f);
        Text = ReadOnlySeString.FromMacroString($"<color({(uint)color})>R: {color.R:0.00} G: {color.G:0.00} B: {color.B:0.00}<color(0)>");
    }
}
