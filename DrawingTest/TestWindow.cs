using System.IO;
using System.Reflection;
using HaselCommon.ImGuiYoga;
using HaselCommon.Services;
namespace DrawingTest;

public class TestWindow : YogaWindow
{
    public TestWindow(WindowManager WindowManager) : base(WindowManager, "TestWindow")
    {
        YogaParser.Load(this, Assembly.GetExecutingAssembly().GetManifestResourceStream("DrawingTest.TestWindow.xml") ?? throw new FileNotFoundException());
    }
}
