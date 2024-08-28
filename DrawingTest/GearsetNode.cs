using System.Numerics;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Components;
using HaselCommon.Services;
using HaselCommon.Yoga;

namespace DrawingTest;

internal static class GearsetNodeStyle
{
    public static Vector2 IconSize = new(34);
    public static Vector2 IconInset = IconSize * 0.08333f;
}

public class GearsetNode : YogaNode
{
    public GearsetNode(ItemService itemService, string id, Gearset gearset) : base(id)
    {
        FlexDirection = YGFlexDirection.Row;
        AlignItems = YGAlign.Center;

        UseEffect(
            gearset.IsValid,
            isValid => Display = isValid && !gearset.Name.CurrentValue.StartsWith("==") ? YGDisplay.Flex : YGDisplay.None,
            doImmediately: false);

        UseEffect(
            gearset.Name,
            name => Display = gearset.IsValid.CurrentValue && !name.StartsWith("==") ? YGDisplay.Flex : YGDisplay.None);

        ChildNodes.Add(new YogaNode
        {
            MarginHorizontal = 10,
            MarginVertical = 4,
            AlignItems = YGAlign.Center,
            OnSetup = (node) =>
            {
                node.ChildNodes.AddRange([
                    new IconNode
                    {
                        Width = GearsetNodeStyle.IconSize.X,
                        Height = GearsetNodeStyle.IconSize.Y,

                        OnSetup = (node) =>
                        {
                            if (node is not IconNode iconNode)
                                return;

                            node.UseEffect(gearset.JobId, jobId => iconNode.IconId = 62100u + jobId);
                        }
                    },
                    new BindableTextNode()
                    {
                        OnSetup = (node) =>
                        {
                            if (node is not BindableTextNode textNode)
                                return;

                            textNode.Text.Value = (gearset.Id + 1).ToString();
                        }
                    }
                ]);
            }
        });

        ChildNodes.Add(new ListNode<GearsetSlot, GearsetSlotNode>(gearset.Slots, (slotIndex, slot) =>
        {
            return new GearsetSlotNode($"Slot{slotIndex}", itemService, slotIndex, gearset, slot);
        })
        {
            FlexDirection = YGFlexDirection.Row,
            FlexWrap = YGWrap.Wrap
        });
    }
}
