using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using HaselCommon.Graphics;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Attributes;
using HaselCommon.Gui.Yoga.Events;
using ImGuiNET;
using Lumina.Text.ReadOnly;
using YogaSharp;

namespace DrawingTest;

public class AlertBox : Node
{
    private AlertBoxPreset _preset = AlertBoxPreset.Info;

    private FontAwesomeIconNode IconNode { get; init; }
    private TextNode TextNode { get; init; }
    private FontAwesomeIconNode DismissNode { get; init; }

    [NodeProp("AlertBox", editable: true)]
    public AlertBoxPreset Preset
    {
        get => _preset;
        set
        {
            _preset = value;

            switch (_preset)
            {
                case AlertBoxPreset.Info:
                    Icon = FontAwesomeIcon.InfoCircle;
                    IconColor = hsl(202.5f, 1, 0.545f);
                    TextColor = hsl(202.5f, 1, 0.545f);
                    break;

                case AlertBoxPreset.Warning:
                    Icon = FontAwesomeIcon.ExclamationCircle;
                    IconColor = hsl(47, 0.881f, 0.537f);
                    TextColor = hsl(47, 0.881f, 0.537f);
                    break;

                case AlertBoxPreset.Error:
                    Icon = FontAwesomeIcon.TimesCircle;
                    IconColor = hsl(345.6f, 0.874f, 0.625f);
                    TextColor = hsl(345.6f, 0.874f, 0.625f);
                    break;
            }
        }
    }

    [NodeProp("AlertBox", editable: true)]
    public Color BackgroundColor { get; set; } = hsl(0, 0, 0.17f);

    [NodeProp("AlertBox", editable: true)]
    public float BackgroundBorderRadius { get; set; } = 3;

    [NodeProp("AlertBox", editable: true)]
    public FontAwesomeIcon Icon
    {
        get => IconNode.Icon;
        set => IconNode.Icon = value;
    }

    [NodeProp("AlertBox", editable: true)]
    public Color? IconColor
    {
        get => IconNode.IconDefaultColor;
        set => IconNode.IconDefaultColor = value;
    }

    [NodeProp("AlertBox", editable: true)]
    public Color? TextColor
    {
        get => TextNode.TextColor;
        set => TextNode.TextColor = value;
    }

    [NodeProp("AlertBox", editable: true)]
    public Color? DismissIconColor
    {
        get => DismissNode.IconDefaultColor;
        set => DismissNode.IconDefaultColor = value;
    }

    [NodeProp("AlertBox", editable: true)]
    public ReadOnlySeString Text
    {
        get => TextNode.Text;
        set => TextNode.Text = value;
    }

    public bool Dismissable
    {
        get => DismissNode.Display == YGDisplay.Flex;
        set => DismissNode.Display = value ? YGDisplay.Flex : YGDisplay.None;
    }

    public AlertBox() : base()
    {
        FlexDirection = YGFlexDirection.Row;
        Padding = 17;
        ColumnGap = 14;
        AlignItems = YGAlign.Center;

        Add(IconNode = new());
        Add(TextNode = new()
        {
            FlexShrink = 1,
            FlexGrow = 1
        });
        Add(DismissNode = new()
        {
            Display = YGDisplay.None,
            Padding = 4,
            Margin = -4,
            Icon = FontAwesomeIcon.Times,
            IconDefaultColor = hsl(0, 0, 0.631f),
            IconHoveredColor = hsl(0, 0, 0.831f),
        });

        DismissNode.AddEventListener<MouseEvent>((sender, evt) =>
        {
            switch (evt.EventType)
            {
                case MouseEventType.MouseHover:
                    {
                        evt.Bubbles = false;
                        ImGui.SetMouseCursor(ImGuiMouseCursor.Hand);
                        using var tooltip = ImRaii.Tooltip();
                        ImGui.TextUnformatted("Dismiss");
                        break;
                    }

                case MouseEventType.MouseClick:
                    evt.Bubbles = false;
                    DispatchEvent(new AlertBoxDismissedEvent());
                    break;
            }
        });

        Preset = AlertBoxPreset.Info;
    }

    public override void DrawContent()
    {
        // draw background
        var pos = ImGui.GetWindowPos() + AbsolutePosition;
        ImGui.GetWindowDrawList().AddRectFilled(pos, pos + ComputedSize, BackgroundColor, BackgroundBorderRadius);
    }
}

public enum AlertBoxPreset
{
    Info,
    Warning,
    Error
}

public class AlertBoxDismissedEvent : YogaEvent;
