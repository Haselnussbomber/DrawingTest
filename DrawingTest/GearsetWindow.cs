using HaselCommon.ImGuiYoga;
using HaselCommon.Services;
namespace DrawingTest;

public class GearsetWindow : YogaWindow
{
    private readonly ItemService itemService;

    public unsafe GearsetWindow(
        WindowManager WindowManager,
        ItemService ItemService,
        GearsetRepository GearsetRepository) : base(WindowManager, "Gearsets")
    {
        itemService = ItemService;

        RootNode = new ListNode<Gearset, GearsetNode>(GearsetRepository.Gearsets, (index, gearset) => new GearsetNode(itemService, $"Gearset{gearset.Id}", gearset));
    }
}
