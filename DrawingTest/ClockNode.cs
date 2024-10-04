using Dalamud.Plugin.Services;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Enums;
using Lumina.Text;

namespace DrawingTest;

public class ClockNode : TextNode
{
    public ClockNode()
    {
        Service.Get<IPluginLog>().Debug("ClockNode ctor");
    }

    public override void UpdateContent()
    {
        if (Display == Display.None)
            return;

        Text = new SeStringBuilder()
            .Append("Current Time is: ")
            .Append(DateTime.Now.ToString("HH:mm:ss"))
            .ToReadOnlySeString();
    }
}
