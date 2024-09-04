using System;
using System.Text;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Attributes;
using Lumina.Text.ReadOnly;

namespace DrawingTest;

public class ClockElement : Text
{
    private DateTime LastDateTime = DateTime.MinValue;

    [NodeRef(".time")]
    private Text? TimeTextNode;

    [NodeProperty]
    public string Format { get; set; } = "HH:mm";

    public override void Update()
    {
        if (IsDirty)
        {
            UpdateRefs();
        }

        TimeTextNode ??= QuerySelector<Text>("#text");

        var now = DateTime.Now;
        if (LastDateTime != now)
        {
            Data = new ReadOnlySeString(Encoding.UTF8.GetBytes(now.ToString(Format)));
            LastDateTime = now;
        }
    }
}
