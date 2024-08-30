using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Core;
using HaselCommon.Services;

namespace DrawingTest;

public class TestWindow : YogaWindow
{
    public TestWindow(WindowManager WindowManager) : base(WindowManager, "TestWindow")
    {
        Context.RegisterType<ClockNode>();
        RootNode = YogaLoader.LoadManifestResource(Context, "DrawingTest.TestWindow.xml");
    }
}
