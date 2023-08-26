// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class ListWithCallback<T>(Action callback) : IList<T>, IReadOnlyList<T>
{
    private readonly List<T> _list = new();
    private readonly Action _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }

    public void Add(T item)
    {
        _list.Add(item);
        _callback();
    }

    public void Clear()
    {
        _list.Clear();
        _callback();
    }

    public bool Contains(T item) => _list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
        _callback();
    }

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    public int IndexOf(T item) => _list.IndexOf(item);

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        _callback();
    }

    public bool Remove(T item)
    {
        var result = _list.Remove(item);
        if (result)
        {
            _callback();
        }

        return result;
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
        _callback();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
