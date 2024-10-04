using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Services;

namespace DrawingTest;

public class TestWindow : YogaWindow
{
    public TestWindow(WindowManager windowManager) : base(windowManager, "TestWindow")
    {
        EnableDebug = true;

        RootNode.FlexDirection = FlexDirection.Row;
        RootNode.ColumnGap = 20;
        RootNode.Add(
            new ClockNode
            {
                MinWidth = StyleLength.Percent(33),
                Height = 60,
            },
            new AnimatedColoredTextNode(),
            new Node()
            {
                FlexDirection = FlexDirection.Row,
                Width = StyleLength.Percent(33),
                Height = 60,
                ColumnGap = 3,
                Children = [
                    new AnimatedBox() {
                        Children = [
                            new TextNode()
                            {
                                Text = "hello",
                            },
                        ]
                    },

                    new TextNode()
                    {
                        Text = "world"
                    }
                ]
            },
            new Node()
            {
                FlexDirection = FlexDirection.Row,
                Width = StyleLength.Percent(33),
                Height = 60,
                ColumnGap = 3
            }
        );
    }
}
