using HaselCommon.ImGuiYoga;
using HaselCommon.Utils;
using HaselCommon.Yoga;
using ImGuiNET;
using R3;
using YGMeasureFuncDelegate = HaselCommon.Yoga.Interop.YGMeasureFuncDelegate;

namespace DrawingTest;

public unsafe class BindableTextNode : YogaNode
{
    public BindableTextNode() : base()
    {
        SetMeasureFunc(new YGMeasureFuncDelegate(Measure));
        NodeType = NodeType.Text;

        Text.Subscribe(_ => IsDirty = true).AddTo(Disposables);
    }

    public BindableTextNode(string id) : this()
    {
        Id = id;
    }

    public ReactiveProperty<string> Text { get; } = new(string.Empty);

    public ReactiveProperty<HaselColor?> TextColor { get; set; } = new(null);

    private YGSize Measure(YGNode* node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
    {
        return ImGui.CalcTextSize(Text.CurrentValue, width);
    }

    public override void Draw()
    {
        OnSetup?.Invoke(this);
        OnSetup = null;

        if (Display == YGDisplay.None)
            return;

        PreDraw();

        if (!ImGuiUtils.IsInViewport(ComputedSize))
        {
            ImGui.Dummy(ComputedSize);
        }
        else
        {
            ImGui.PushTextWrapPos(CumulativePosition.X + ComputedWidth);

            if (TextColor.CurrentValue != null)
                ImGui.PushStyleColor(ImGuiCol.Text, (uint)TextColor.CurrentValue);

            // TODO: push font

            ImGui.TextUnformatted(Text.CurrentValue);

            if (TextColor.CurrentValue != null)
                ImGui.PopStyleColor();

            ImGui.PopTextWrapPos();
        }

        PostDraw();
    }
}
