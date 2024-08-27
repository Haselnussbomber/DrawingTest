using System.Numerics;
using HaselCommon.ImGuiYoga;
using HaselCommon.ImGuiYoga.Components;
using HaselCommon.Services;
using HaselCommon.Utils;
using HaselCommon.Yoga;
using R3;

namespace DrawingTest;

public class GearsetNode : Node
{
    private static Vector2 IconSize = new(34);
    private static Vector2 IconInset = IconSize * 0.08333f;

    public GearsetNode(ItemService itemService, string id, Gearset gearset) : base(id)
    {
        FlexDirection = FlexDirection.Row;
        AlignItems = Align.Center;

        gearset.IsValid
            .Subscribe(isValid => Display = isValid && !gearset.Name.CurrentValue.StartsWith("==") ? Display.Flex : Display.None)
            .AddTo(Disposables);

        gearset.Name
            .Do(name => Display = gearset.IsValid.CurrentValue && !name.StartsWith("==") ? Display.Flex : Display.None)
            .Subscribe()
            .AddTo(Disposables);

        var classNode = new Node
        {
            MarginHorizontal = 10,
            MarginVertical = 4,
            AlignItems = Align.Center,
            ChildNodes = [
                new IconNode
                {
                    Width = IconSize.X,
                    Height = IconSize.Y,

                    OnSetup = (node) =>
                    {
                        if (node is not IconNode iconNode)
                            return;

                        gearset.JobId
                            .Do(jobId => iconNode.IconId = 62100u + jobId)
                            .Subscribe()
                            .AddTo(node.Disposables);
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
            ]
        };

        ChildNodes.Add(classNode);

        var slotList = new Node
        {
            FlexDirection = FlexDirection.Row,
            FlexWrap = Wrap.Wrap
        };

        for (var slotIndex = 0; slotIndex < gearset.Slots.CurrentValue.Length; slotIndex++)
        {
            // skip obsolete belt slot
            if (slotIndex == 5)
                continue;

            slotList.ChildNodes.Add(CreateSlotNode(itemService, slotIndex, gearset, gearset.Slots.CurrentValue[slotIndex]));
        }

        ChildNodes.Add(slotList);
    }

    private Node CreateSlotNode(ItemService itemService, int slotIndex, Gearset gearset, GearsetSlot slot)
    {
        return new Node($"Slot{slotIndex}")
        {
            AlignItems = Align.Center,
            ChildNodes = [
                new Node
                {
                    Width = IconSize.X,
                    Height = IconSize.Y,
                    PositionType = PositionType.Relative,
                    ChildNodes = [
                        new UldPartNode
                        {
                            PositionType = PositionType.Absolute,
                            PositionTop = 0,
                            PositionLeft = 0,
                            Width = IconSize.X,
                            Height = IconSize.Y,
                            UldName = "Character",
                            PartListId = 8,
                            PartIndex = 0,

                            OnSetup = (node) =>
                            {
                                slot.Item
                                    .Do(item => node.Display = item != null ? Display.None : Display.Flex)
                                    .Subscribe()
                                    .AddTo(node.Disposables);
                            }
                        },
                        new UldPartNode
                        {
                            PositionType = PositionType.Absolute,
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
                                slot.Item
                                    .Do(item => node.Display = item != null ? Display.None : Display.Flex)
                                    .Subscribe()
                                    .AddTo(node.Disposables);
                            }
                        },
                        new UldPartNode
                        {
                            PositionType = PositionType.Absolute,
                            PositionTop = 0,
                            PositionLeft = 0,
                            Width = IconSize.X,
                            Height = IconSize.Y,
                            UldName = "Character",
                            PartListId = 7,
                            PartIndex = 4,

                            OnSetup = (node) =>
                            {
                                slot.Item
                                    .Do(item => node.Display = item != null ? Display.Flex : Display.None)
                                    .Subscribe()
                                    .AddTo(node.Disposables);
                            }
                        },
                        new IconNode
                        {
                            PositionType = PositionType.Absolute,
                            PositionTop = IconInset.X,
                            PositionLeft = IconInset.Y,
                            Width = IconSize.X - IconInset.X * 2,
                            Height = IconSize.Y - IconInset.Y * 2,
                            IconId = 0,

                            OnSetup = (node) =>
                            {
                                if (node is not IconNode iconNode)
                                    return;

                                slot.Item
                                    .Do(item => node.Display = item != null ? Display.Flex : Display.None)
                                    .Subscribe()
                                    .AddTo(node.Disposables);

                                slot.IconId
                                    .Do(iconId => iconNode.IconId = iconId)
                                    .Subscribe()
                                    .AddTo(node.Disposables);
                            }
                        },
                        new UldPartNode
                        {
                            PositionType = PositionType.Absolute,
                            PositionTop = 0,
                            PositionLeft = 0,
                            Width = IconSize.X,
                            Height = IconSize.Y,
                            UldName = "Character",
                            PartListId = 7,
                            PartIndex = 0,

                            OnSetup = (node) =>
                            {
                                slot.Item
                                    .Do(item => node.Display = item != null ? Display.Flex : Display.None)
                                    .Subscribe()
                                    .AddTo(node.Disposables);
                            }
                        },
                    ]
                },
                new BindableTextNode
                {
                    OnSetup = (node) => {
                        if (node is not BindableTextNode textNode)
                            return;

                        slot.Item
                            .Do(item => {
                                node.Display = item != null ? Display.Flex : Display.None;

                                if (item != null)
                                {
                                    textNode.TextColor.Value = itemService.GetItemLevelColor(gearset.JobId.CurrentValue, item, Colors.Red, Colors.Yellow, Colors.Green);
                                    textNode.Text.Value = item.LevelItem.Row.ToString();
                                }
                            })
                            .Subscribe()
                            .AddTo(node.Disposables);
                    }
                }
            ]
        };
    }
}
