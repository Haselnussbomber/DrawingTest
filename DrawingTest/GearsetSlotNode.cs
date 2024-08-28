using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Components;
using HaselCommon.Services;
using HaselCommon.Utils;
using HaselCommon.Yoga;

namespace DrawingTest;

public class GearsetSlotNode : YogaNode
{
    public GearsetSlotNode(string id, ItemService itemService, int slotIndex, Gearset gearset, GearsetSlot slot)
    {
        Id = id;

        AlignItems = YGAlign.Center;

        var IconSize = GearsetNodeStyle.IconSize;
        var IconInset = GearsetNodeStyle.IconInset;

        ChildNodes.Add(new YogaNode
        {
            Width = IconSize.X,
            Height = IconSize.Y,
            PositionType = YGPositionType.Relative,
            OnSetup = (node) =>
            {
                node.ChildNodes.AddRange([
                    new UldPartNode
                    {
                        PositionType = YGPositionType.Absolute,
                        PositionTop = 0,
                        PositionLeft = 0,
                        Width = IconSize.X,
                        Height = IconSize.Y,
                        UldName = "Character",
                        PartListId = 8,
                        PartIndex = 0,

                        OnSetup = (node) =>
                        {
                            node.UseEffect(slot.Item, item => node.Display = item != null ? YGDisplay.None : YGDisplay.Flex);
                        }
                    },
                    new UldPartNode
                    {
                        PositionType = YGPositionType.Absolute,
                        PositionTop = IconInset.X,
                        PositionLeft = IconInset.Y,
                        Width = IconSize.X - IconInset.X * 2,
                        Height = IconSize.Y - IconInset.Y * 2,
                        UldName = "Character",
                        PartListId = 12,
                        PartIndex = (uint)(17 + slotIndex switch
                        {
                            12 => 11, // left ring
                            _ => slotIndex,
                        }),

                        OnSetup = (node) =>
                        {
                            node.UseEffect(slot.Item, item => node.Display = item != null ? YGDisplay.None : YGDisplay.Flex);
                        }
                    },
                    new UldPartNode
                    {
                        PositionType = YGPositionType.Absolute,
                        PositionTop = 0,
                        PositionLeft = 0,
                        Width = IconSize.X,
                        Height = IconSize.Y,
                        UldName = "Character",
                        PartListId = 7,
                        PartIndex = 4,

                        OnSetup = (node) =>
                        {
                            node.UseEffect(slot.Item, item => node.Display = item == null ? YGDisplay.None : YGDisplay.Flex);
                        }
                    },
                    new IconNode
                    {
                        PositionType = YGPositionType.Absolute,
                        PositionTop = IconInset.X,
                        PositionLeft = IconInset.Y,
                        Width = IconSize.X - IconInset.X * 2,
                        Height = IconSize.Y - IconInset.Y * 2,
                        IconId = 0,

                        OnSetup = (node) =>
                        {
                            if (node is not IconNode iconNode)
                                return;

                            node.UseEffect(slot.Item, item => node.Display = item == null ? YGDisplay.None : YGDisplay.Flex);
                            node.UseEffect(slot.IconId, iconId => iconNode.IconId = iconId);
                        }
                    },
                    new UldPartNode
                    {
                        PositionType = YGPositionType.Absolute,
                        PositionTop = 0,
                        PositionLeft = 0,
                        Width = IconSize.X,
                        Height = IconSize.Y,
                        UldName = "Character",
                        PartListId = 7,
                        PartIndex = 0,

                        OnSetup = (node) =>
                        {
                            node.UseEffect(slot.Item, item => node.Display = item == null ? YGDisplay.None : YGDisplay.Flex);
                        }
                    },
                ]);
            }
        });

        ChildNodes.Add(new BindableTextNode
        {
            OnSetup = (node) =>
            {
                if (node is not BindableTextNode textNode)
                    return;

                node.UseEffect(slot.Item, item =>
                {
                    node.Display = item != null ? YGDisplay.Flex : YGDisplay.None;

                    if (item != null)
                    {
                        textNode.TextColor.Value = itemService.GetItemLevelColor(gearset.JobId.CurrentValue, item, Colors.Red, Colors.Yellow, Colors.Green);
                        textNode.Text.Value = item.LevelItem.Row.ToString();
                    }
                });
            }
        });
    }
}
