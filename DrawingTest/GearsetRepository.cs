using System;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using HaselCommon;
using HaselCommon.Services;
using ObservableCollections;
using R3;

namespace DrawingTest;

public class GearsetRepository : IDisposable
{
    private const int GearsetCount = 100;

    private readonly CompositeDisposable disposables = [];
    private readonly Gearset[] _gearsets = new Gearset[GearsetCount];
    private readonly ObservableList<Gearset> _existingGearsets = [];

    public IReadOnlyObservableList<Gearset> Gearsets => _existingGearsets;

    public unsafe GearsetRepository(ItemService itemService, ExcelService excelService)
    {
        var rgm = RaptureGearsetModule.Instance();

        for (byte index = 0; index < GearsetCount; index++)
        {
            var gearset = new Gearset(itemService, excelService, index).AddTo(disposables);

            _gearsets[index] = gearset;

            gearset.IsValid
                .Do(isValid =>
                {
                    Service.Get<IPluginLog>().Debug($"{gearset.Id} is valid? {isValid}");
                    if (isValid)
                        _existingGearsets.Add(gearset);
                    else
                        _existingGearsets.Remove(gearset);
                })
                .Subscribe()
                .AddTo(disposables);
        }
    }

    public void Dispose()
    {
        disposables.Dispose();
        GC.SuppressFinalize(this);
    }
}
