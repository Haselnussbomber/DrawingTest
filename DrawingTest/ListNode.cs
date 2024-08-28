using System;
using System.Collections.Generic;
using HaselCommon.ImGuiYoga;
using ObservableCollections;
using R3;

namespace DrawingTest;

public unsafe class ListNode<T, TNode> : YogaNode
    where TNode : YogaNode
{
    private readonly Func<int, T, TNode> Factory;
    private readonly IComparer<T>? Comparer; // TODO: implement

    public ListNode(IReadOnlyObservableList<T> list, Func<int, T, TNode> factory, IComparer<T>? comparer = null) : base()
    {
        Factory = factory;
        Comparer = comparer;

        for (var i = 0; i < list.Count; i++)
        {
            ChildNodes.Add(factory(i, list[i]));
        }

        list.ObserveAdd()
            .Subscribe(OnCollectionAddEvent)
            .AddTo(Disposables);

        list.ObserveRemove()
            .Subscribe(OnCollectionRemoveEvent)
            .AddTo(Disposables);

        list.ObserveMove()
            .Subscribe(OnCollectionMoveEvent)
            .AddTo(Disposables);

        // list.ObserveReplace()
        //     .Subscribe(OnCollectionReplaceEvent)
        //     .AddTo(Disposables);

        list.ObserveReset()
            .Subscribe(OnCollectionResetEvent)
            .AddTo(Disposables);
    }

    public ListNode(string id, IReadOnlyObservableList<T> list, Func<int, T, TNode> factory, IComparer<T>? comparer = null) : this(list, factory, comparer)
    {
        Id = id;
    }

    private void OnCollectionAddEvent(CollectionAddEvent<T> evt)
    {
        ChildNodes.Add(Factory(evt.Index, evt.Value));
    }

    private void OnCollectionRemoveEvent(CollectionRemoveEvent<T> evt)
    {
        ChildNodes.RemoveAt(evt.Index);
    }

    private void OnCollectionMoveEvent(CollectionMoveEvent<T> evt)
    {
        ChildNodes.Move(evt.OldIndex, evt.NewIndex);
    }

    // private void OnCollectionReplaceEvent(CollectionReplaceEvent<T> evt)
    // {
    //     throw new NotImplementedException();
    // }

    private void OnCollectionResetEvent(Unit unit)
    {
        ChildNodes.Clear();
    }
}
