using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using HaselCommon.Graphics;
using HaselCommon.Gui.Yoga;
using HaselCommon.Gui.Yoga.Attributes;
using HaselCommon.Gui.Yoga.Enums;
using HaselCommon.Gui.Yoga.Events;
using ImGuiNET;
using Lumina.Text.ReadOnly;

namespace DrawingTest;

public class AlertBox : Node
{
    private readonly FontAwesomeIconNode _iconNode;
    private readonly TextNode _textNode;
    private readonly FontAwesomeIconNode _closeNode;
    private AlertBoxPreset _preset = AlertBoxPreset.Info;

    [NodeProp("AlertBox", editable: true)]
    public Color BackgroundColor { get; set; } = hsl(0, 0, 0.17f);

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
    public FontAwesomeIcon Icon
    {
        get => _iconNode.Icon;
        set => _iconNode.Icon = value;
    }

    [NodeProp("AlertBox", editable: true)]
    public Color? IconColor
    {
        get => _iconNode.IconDefaultColor;
        set => _iconNode.IconDefaultColor = value;
    }

    [NodeProp("AlertBox", editable: true)]
    public Color? TextColor
    {
        get => _textNode.TextColor;
        set => _textNode.TextColor = value;
    }

    [NodeProp("AlertBox", editable: true)]
    public Color? CloseIconColor
    {
        get => _closeNode.IconDefaultColor;
        set => _closeNode.IconDefaultColor = value;
    }

    [NodeProp("AlertBox", editable: true)]
    public ReadOnlySeString Text
    {
        get => _textNode.Text;
        set => _textNode.Text = value;
    }

    public bool Closable
    {
        get => _closeNode.Display == Display.Flex;
        set => _closeNode.Display = value ? Display.Flex : Display.None;
    }

    public AlertBox() : base()
    {
        FlexDirection = FlexDirection.Row;
        PaddingAll = 17;
        ColumnGap = 14;
        AlignItems = Align.Center;

        Add(_iconNode = new());
        Add(_textNode = new()
        {
            FlexShrink = 1,
            FlexGrow = 1
        });
        Add(_closeNode = new()
        {
            Display = Display.None,
            Icon = FontAwesomeIcon.Times,
            IconDefaultColor = hsl(0, 0, 0.631f),
            IconHoveredColor = hsl(0, 0, 0.831f),
        });

        AddEventListener<MouseEvent>((sender, evt) =>
        {
            if (evt.EventType == MouseEventType.MouseHover && sender == _closeNode)
            {
                ImGui.SetMouseCursor(ImGuiMouseCursor.Hand);
                using var tooltip = ImRaii.Tooltip();
                ImGui.TextUnformatted("Close");
            }
            if (evt.EventType == MouseEventType.MouseClick && sender == _closeNode)
            {
                evt.Bubbles = false;
                DispatchEvent(new CloseEvent());
            }
        });

        Preset = AlertBoxPreset.Info;
    }

    public override void DrawContent()
    {
        var pos = ImGui.GetWindowPos() + AbsolutePosition;
        ImGui.GetWindowDrawList().AddRectFilled(pos, pos + ComputedSize, BackgroundColor, 3);
    }
}

public enum AlertBoxPreset
{
    Info,
    Warning,
    Error
}

public class CloseEvent : YogaEvent;
