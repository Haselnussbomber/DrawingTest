using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Services;

namespace DrawingTest;

public class TestWindow : YogaWindow
{
    public TestWindow(WindowManager windowManager) : base(windowManager, "TestWindow")
    {
        EnableDebug = true;

        RootNode.Gap = 10;
        RootNode.Add(
            new AlertBox()
            {
                Preset = AlertBoxPreset.Info,
                Text = "This is an AlertBox with the Info preset.",
                Closable = true,
                CloseCallback = (alertBox) => alertBox.Parent?.Remove(alertBox) // this throws
            },

            new AlertBox()
            {
                Preset = AlertBoxPreset.Warning,
                Text = "This is an AlertBox with the Warning preset."
            },

            new AlertBox()
            {
                Preset = AlertBoxPreset.Error,
                Text = "This is an AlertBox with the Error preset."
            },

            new Node()
            {
                FlexDirection = FlexDirection.Row,
                ColumnGap = 20,
                Children = [
                    new ClockNode
                    {
                        MinWidth = StyleLength.Percent(33),
                        Height = 60,
                    }
                ]
            }
        );
    }
}
