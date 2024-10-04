using System;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Math;

namespace DrawingTest;

public class AnimatedBox : Node
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
    }
}
