using Dalamud.Plugin.Services;
using HaselCommon.Gui.Yoga;
using Lumina.Text;
using YogaSharp;

namespace DrawingTest;

public class ClockNode : TextNode
{
    public ClockNode() : base()
    {
        Service.Get<IPluginLog>().Debug("ClockNode ctor");
    }

    public override void UpdateContent()
    {
        if (Display == YGDisplay.None)
            return;

        Text = new SeStringBuilder()
            .Append("Current Time is: ")
            .Append(DateTime.Now.ToString("HH:mm:ss"))
            .ToReadOnlySeString();
    }
}
