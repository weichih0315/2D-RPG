using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomStack<T> : Stack<T> {

    public event System.Action OnPush;
    public event System.Action OnPop;
    public event System.Action OnClear;

    public CustomStack(CustomStack<T> items) : base(items)
    {

    }

    public CustomStack()
    {

    }

    public new void Push(T item)
    {
        base.Push(item);

        if (OnPush != null)
            OnPush();
    }

    public new T Pop()
    {
        T item = base.Pop();

        if (OnPop != null)
            OnPop();

        return item;
    }

    public new void Clear()
    {
        base.Clear();

        if (OnClear != null)
            OnClear();
    }
}
