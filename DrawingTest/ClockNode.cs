using System;
using HaselCommon.ImGuiYoga.Attributes;
using HaselCommon.ImGuiYoga.Components;

namespace DrawingTest;

[YogaNode("clock")]
public partial class ClockNode : TextNode
{
    private TextNode? TimeTextNode; // TODO: let generator create refs

    public string Format { get; set; } = "HH:mm";

    private DateTime LastDateTime = DateTime.MinValue;

    public override void ApplyXmlAttribute(string name, string value)
    {
        switch (name)
        {
            case "format":
                Format = value;
                break;

            default:
                base.ApplyXmlAttribute(name, value);
                break;
        }
    }

    public override void Update()
    {
        TimeTextNode ??= GetNodeById<TextNode>("text");

        var now = DateTime.Now;
        if (LastDateTime != now)
        {
            Text = now.ToString(Format);
            LastDateTime = now;
            IsDirty = true;
        }
    }
}
