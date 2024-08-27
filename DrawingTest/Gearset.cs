using System;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using HaselCommon.Services;
using Lumina.Excel.GeneratedSheets;
using R3;

namespace DrawingTest;

public record Gearset : IEquatable<Gearset>, IComparable<Gearset>, IDisposable
{
    private const int SlotCount = 13;
    private readonly ItemService itemService;
    private readonly ExcelService excelService;
    private readonly CompositeDisposable disposables = [];

    private readonly ReactiveProperty<bool> _isValid = new();
    private readonly ReactiveProperty<string> _name = new(string.Empty);
    private readonly ReactiveProperty<byte> _jobId = new();
    private readonly ReactiveProperty<short> _itemLevel = new();
    private readonly ReactiveProperty<bool> _isCurrent = new();
    private readonly ReactiveProperty<bool> _hasMissingItems = new();
    private readonly ReactiveProperty<bool> _appearanceDiffers = new();
    private readonly ReactiveProperty<bool> _isMainHandMissing = new();
    private readonly ReactiveProperty<byte> _glamourSetLink = new();
    private readonly ReactiveProperty<GearsetSlot[]> _slots = new([]);

    public byte Id { get; init; }
    public ReadOnlyReactiveProperty<bool> IsValid => _isValid;
    public ReadOnlyReactiveProperty<string> Name => _name;
    public ReadOnlyReactiveProperty<byte> JobId => _jobId;
    public ReadOnlyReactiveProperty<short> ItemLevel => _itemLevel;
    public ReadOnlyReactiveProperty<bool> IsCurrent => _isCurrent;
    public ReadOnlyReactiveProperty<bool> HasMissingItems => _hasMissingItems;
    public ReadOnlyReactiveProperty<bool> AppearanceDiffers => _appearanceDiffers;
    public ReadOnlyReactiveProperty<bool> IsMainHandMissing => _isMainHandMissing;
    public ReadOnlyReactiveProperty<byte> GlamourSetLink => _glamourSetLink;
    public ReadOnlyReactiveProperty<GearsetSlot[]> Slots => _slots;

    public Gearset(ItemService itemService, ExcelService excelService, byte id)
    {
        this.itemService = itemService;
        this.excelService = excelService;
        Id = id;

        Update();

        Observable
            .IntervalFrame(250)
            //.EveryUpdate()
            .Subscribe(_ => Update())
            .AddTo(disposables);
    }

    public void Dispose()
    {
        Disposable.Dispose(
            disposables,
            _isValid,
            _name,
            _jobId,
            _itemLevel,
            _isCurrent,
            _hasMissingItems,
            _appearanceDiffers,
            _isMainHandMissing,
            _glamourSetLink);

        GC.SuppressFinalize(this);
    }

    private unsafe void Update()
    {
        var gsm = RaptureGearsetModule.Instance();
        var isValid = gsm->IsValidGearset(Id);
        if (!isValid)
        {
            _isValid.Value = false;
            return;
        }

        var gearset = gsm->GetGearset(Id);
        if (gearset == null)
            return;

        _isValid.Value = gearset->Flags.HasFlag(RaptureGearsetModule.GearsetFlag.Exists);
        _name.Value = gearset->NameString;
        _jobId.Value = gearset->ClassJob;
        _itemLevel.Value = gearset->ItemLevel;
        _isCurrent.Value = gsm->CurrentGearsetIndex == Id && gearset->ClassJob > 0;
        _isMainHandMissing.Value = gearset->Flags.HasFlag(RaptureGearsetModule.GearsetFlag.MainHandMissing);
        _glamourSetLink.Value = gearset->GlamourSetLink;

        // Check for missing items.
        var hasMissingItems = false;
        var appearanceDiffers = false;

        foreach (var item in gearset->Items)
        {
            if (item.Flags.HasFlag(RaptureGearsetModule.GearsetItemFlag.ItemMissing))
            {
                hasMissingItems = true;
            }

            if (item.Flags.HasFlag(RaptureGearsetModule.GearsetItemFlag.AppearanceDiffers))
            {
                appearanceDiffers = true;
            }
        }

        _appearanceDiffers.Value = appearanceDiffers;
        _hasMissingItems.Value = hasMissingItems;

        if (_slots.CurrentValue.Length == 0)
        {
            _slots.Value = new GearsetSlot[SlotCount];
            for (var i = 0; i < SlotCount; i++)
            {
                _slots.CurrentValue[i] = new GearsetSlot(itemService, excelService, gearset->Items[i]).AddTo(disposables);
            }
        }
        else
        {
            for (var i = 0; i < SlotCount; i++)
            {
                _slots.CurrentValue[i].Update(gearset->Items[i]);
            }
        }
    }

    public int CompareTo(Gearset? other)
    {
        return other == null ? 1 : Comparer<byte>.Default.Compare(Id, other.Id);
    }
}

public record GearsetSlot : IDisposable
{
    private readonly ItemService itemService;
    private readonly ExcelService excelService;

    private readonly ReactiveProperty<uint> _itemId = new();
    private readonly ReactiveProperty<uint> _glamourId = new();
    private readonly ReactiveProperty<byte> _stain0Id = new();
    private readonly ReactiveProperty<byte> _stain1Id = new();
    private readonly ReactiveProperty<ushort[]> _materia = new();
    private readonly ReactiveProperty<byte[]> _materiaGrades = new();
    private readonly ReactiveProperty<RaptureGearsetModule.GearsetItemFlag> _flags = new();
    private readonly ReactiveProperty<uint> _iconId = new();
    private readonly ReactiveProperty<Item?> _item = new();

    public ReadOnlyReactiveProperty<uint> ItemId => _itemId;
    public ReadOnlyReactiveProperty<uint> GlamourId => _glamourId ;
    public ReadOnlyReactiveProperty<byte> Stain0Id => _stain0Id ;
    public ReadOnlyReactiveProperty<byte> Stain1Id => _stain1Id ;
    public ReadOnlyReactiveProperty<ushort[]> Materia => _materia ;
    public ReadOnlyReactiveProperty<byte[]> MateriaGrades => _materiaGrades ;
    public ReadOnlyReactiveProperty<RaptureGearsetModule.GearsetItemFlag> Flags => _flags ;
    public ReadOnlyReactiveProperty<uint> IconId => _iconId ;
    public ReadOnlyReactiveProperty<Item?> Item => _item ;

    public GearsetSlot(ItemService ItemService, ExcelService ExcelService, RaptureGearsetModule.GearsetItem GearsetItem)
    {
        itemService = ItemService;
        excelService = ExcelService;
        Update(GearsetItem);
    }

    public void Dispose()
    {
        Disposable.Dispose(
            _itemId,
            _glamourId,
            _stain0Id,
            _stain1Id,
            _materia,
            _materiaGrades,
            _flags,
            _iconId,
            _item);

        GC.SuppressFinalize(this);
    }

    internal unsafe void Update(RaptureGearsetModule.GearsetItem GearsetItem)
    {
        _itemId.Value = GearsetItem.ItemId;
        _glamourId.Value = GearsetItem.GlamourId;
        _stain0Id.Value = GearsetItem.Stain0Id;
        _stain1Id.Value = GearsetItem.Stain1Id;
        _materia.Value = GearsetItem.Materia.ToArray();
        _materiaGrades.Value = GearsetItem.MateriaGrades.ToArray();
        _flags.Value = GearsetItem.Flags;
        _iconId.Value = GearsetItem.ItemId == 0 ? 0 : itemService.GetIconId(GearsetItem.ItemId);
        _item.Value = GearsetItem.ItemId == 0 ? null : excelService.GetRow<Item>(GearsetItem.ItemId);
    }
}
