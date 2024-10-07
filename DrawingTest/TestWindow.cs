using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Gui.Yoga.Events;
using HaselCommon.Services;

namespace DrawingTest;

public class TestWindow : YogaWindow
{
    private readonly AlertBox _infoBox;
    private readonly ButtonNode _buttonNode;

    public TestWindow(WindowManager windowManager) : base(windowManager, "TestWindow")
    {
        EnableDebug = true;

        RootNode.Gap = 10;
        RootNode.Add(
            _infoBox = new AlertBox()
            {
                Preset = AlertBoxPreset.Info,
                Text = "This is a dismissable AlertBox with the Info preset.",
                Dismissable = true
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
                    },
                    _buttonNode = new ButtonNode
                    {
                        Display = Display.None,
                        Text = "Show info alert"
                    }
                ]
            }
        );

        _infoBox.AddEventListener<AlertBoxDismissedEvent>((sender, evt) =>
        {
            _infoBox.Display = Display.None;
            _buttonNode.Display = Display.Flex;
        });

        _buttonNode.AddEventListener<MouseEvent>((sender, evt) =>
        {
            if (evt.EventType == MouseEventType.MouseClick)
            {
                _buttonNode.Display = Display.None;
                _infoBox.Display = Display.Flex;
            }
        });
    }
}
