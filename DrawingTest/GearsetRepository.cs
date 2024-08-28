using System;
using HaselCommon.Services;
using ObservableCollections;
using R3;

namespace DrawingTest;

public class GearsetRepository : IDisposable
{
    private const int GearsetCount = 100;

    private readonly CompositeDisposable disposables = [];

    // public ISynchronizedView<Gearset, Gearset> Gearsets { get; }
    public IReadOnlyObservableList<Gearset> Gearsets { get; }

    public unsafe GearsetRepository(ItemService itemService, ExcelService excelService)
    {
        var list = new ObservableList<Gearset>();

        for (byte index = 0; index < GearsetCount; index++)
        {
            list.Add(new Gearset(itemService, excelService, index).AddTo(disposables));
        }

        Gearsets = list;

        /*
        Gearsets = _existingGearsets.CreateSortedView(
            gearset => gearset.Id,
            gearset => gearset,
            comparer: Comparer<Gearset>.Default)
            .AddTo(disposables);

        Gearsets.AttachFilter((gearset, _) => gearset.IsValid.CurrentValue);
        */
    }

    public void Dispose()
    {
        disposables.Dispose();
        GC.SuppressFinalize(this);
    }
}
