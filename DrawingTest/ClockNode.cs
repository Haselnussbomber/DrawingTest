using System;
using System.Xml;
using HaselCommon.ImGuiYoga.Components;
using HaselCommon.ImGuiYoga.Core;

namespace DrawingTest;

public class ClockNode : TextNode
{
    public string Format { get; set; } = "HH:mm";

    private DateTime LastDateTime = DateTime.MinValue;

    public override void ApplyXmlAttribute(XmlAttribute attr, YogaContext context, XmlNode node)
    {
        switch (attr.Name)
        {
            case "format":
                Format = attr.Value;
                break;

            default:
                base.ApplyXmlAttribute(attr, context, node);
                break;
        }
    }

    public override void Update()
    {
        var now = DateTime.Now;
        if (LastDateTime != now)
        {
            Text = now.ToString(Format);
            LastDateTime = now;
            IsDirty = true;
        }
    }
}
