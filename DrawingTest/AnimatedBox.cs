using FFXIVClientStructs.FFXIV.Client.System.Framework;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Attributes;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Math;

namespace DrawingTest;

public class AnimatedBox : Node
{
    [NodeProp("Animation")]
    public float AnimationTimestamp { get; set; }

    [NodeProp("Animation")]
    public float AnimationDirection { get; set; } = 1f;

    private float _lastValue;

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
    }
}
