using System;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Elements;

namespace DrawingTest;

public partial class ClockElement(Document document) : TextElement(document)
{
    public static new string Tag => "clock";

    private TextElement? TimeTextNode; // TODO: let generator create refs

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
        TimeTextNode ??= GetNodeById<TextElement>("text");

        var now = DateTime.Now;
        if (LastDateTime != now)
        {
            Text = now.ToString(Format);
            LastDateTime = now;
            IsDirty = true;
        }
    }
}
