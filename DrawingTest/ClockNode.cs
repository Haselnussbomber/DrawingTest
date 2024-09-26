using System;
using System.Text;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Attributes;
using Lumina.Text.ReadOnly;

namespace DrawingTest;

public class ClockNode : Node
{
    private DateTime LastDateTime = DateTime.MinValue;

    [NodeRef("#time")]
    private readonly Node? TimeTextNode;

    public string Format => Attributes["format"] ?? "HH:mm";

    public override void Setup()
    {
        LoadManifestResource("DrawingTest.ClockNode.xml");
    }

    public override void Update()
    {
        base.Update();

        var now = DateTime.Now;
        if (TimeTextNode is Text textNode && LastDateTime != now)
        {
            textNode.Data = new ReadOnlySeString(Encoding.UTF8.GetBytes(now.ToString(Format)));
            LastDateTime = now;
        }
    }
}
