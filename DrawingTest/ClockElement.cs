using System;
using System.Text;
using HaselCommon.ImGuiYoga;
using Lumina.Text.ReadOnly;

namespace DrawingTest;

public partial class ClockElement : Text
{
    public override string NodeName => "clock";

    private Text? TimeTextNode; // TODO: let generator create refs

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
        TimeTextNode ??= GetNodeById<Text>("text");

        var now = DateTime.Now;
        if (LastDateTime != now)
        {
            Data = new ReadOnlySeString(Encoding.UTF8.GetBytes(now.ToString(Format)));
            LastDateTime = now;
            IsDirty = true;
        }
    }
}
