using System;
using System.Globalization;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Attributes;
using HaselCommon.Utils;

namespace DrawingTest;

public class AnimatedNode : Node
{
    private float t;
    private float direction = 1f;

    [NodeRef("*")]
    private readonly Text? TextNode;

    public override void Setup()
    {
        LoadManifestResource("DrawingTest.AnimatedNode.xml");
    }

    public override unsafe void Update()
    {
        base.Update();

        if (TextNode == null)
            return;

        t += MathUtils.Clamp01(Framework.Instance()->FrameDeltaTime / 2f * direction);
        if (t == 0 || t == 1)
            direction = -direction;

        var value = -(MathF.Cos(MathF.PI * t) - 1f) / 2f; // InOutSine

        var stringValue = $"{(value * 20).ToString("0", CultureInfo.InvariantCulture)}px";

        Style["padding-top"] = stringValue;
        TextNode.TextValue = stringValue;
    }
}
