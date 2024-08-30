using System;
using System.Xml;
using HaselCommon.ImGuiYoga.Attributes;
using HaselCommon.ImGuiYoga.Components;

namespace DrawingTest;

[YogaNode("clock")]
public partial class ClockNode : TextNode
{
    private TextNode? TimeTextNode; // TODO: let generator create refs

    public string Format { get; set; } = "HH:mm";

    private DateTime LastDateTime = DateTime.MinValue;

    public override void ApplyXmlAttribute(XmlAttribute attr, XmlNode node)
    {
        switch (attr.Name)
        {
            case "format":
                Format = attr.Value;
                break;

            default:
                base.ApplyXmlAttribute(attr, node);
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
