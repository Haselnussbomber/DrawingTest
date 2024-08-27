using System.Collections.Generic;
using Dalamud.Plugin.Services;
using HaselCommon;
using HaselCommon.ImGuiYoga;
using HaselCommon.Services;
using ObservableCollections;
using R3;
namespace DrawingTest;

public class GearsetWindow : YogaWindow
{
    private readonly ItemService itemService;
    private readonly Node GearsetList;
    private readonly Dictionary<Gearset, GearsetNode> Gearset2Node = [];

    public unsafe GearsetWindow(
        WindowManager WindowManager,
        ItemService ItemService,
        GearsetRepository GearsetRepository) : base(WindowManager, "Gearsets")
    {
        itemService = ItemService;

        GearsetList = new Node("GearsetList");
        RootNode = new Node
        {
            ChildNodes = [GearsetList]
        };

        foreach (var gearset in GearsetRepository.Gearsets)
        {
            CreateGearsetNode(gearset);
        }

        GearsetRepository.Gearsets
            .ObserveAdd()
            .Subscribe((evt) => CreateGearsetNode(evt.Value))
            .AddTo(Disposables);

        GearsetRepository.Gearsets
            .ObserveRemove()
            .Subscribe((evt) => RemoveGearsetNode(evt.Value))
            .AddTo(Disposables);
    }

    private void CreateGearsetNode(Gearset gearset)
    {
        var gearsetNode = new GearsetNode(itemService, $"Gearset{gearset.Id}", gearset);
        Service.Get<IPluginLog>().Debug($"Adding Gearset{gearset.Id}");
        GearsetList.ChildNodes.Add(gearsetNode);
        Gearset2Node.Add(gearset, gearsetNode);
    }

    private void RemoveGearsetNode(Gearset value)
    {
        if (!Gearset2Node.TryGetValue(value, out var gearsetNode))
            return;

        GearsetList.ChildNodes.Remove(gearsetNode);
        Gearset2Node.Remove(value);
    }
}
