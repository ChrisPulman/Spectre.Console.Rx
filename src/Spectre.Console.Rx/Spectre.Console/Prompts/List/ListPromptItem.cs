// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class ListPromptItem<T>(T data, ListPromptItem<T>? parent = null) : IMultiSelectionItem<T>
    where T : notnull
{
    public T Data { get; } = data;

    public ListPromptItem<T>? Parent { get; } = parent;

    public List<ListPromptItem<T>> Children { get; } = new List<ListPromptItem<T>>();

    public int Depth { get; } = CalculateDepth(parent);

    public bool IsSelected { get; set; }

    public bool IsGroup => Children.Count > 0;

    public IMultiSelectionItem<T> Select()
    {
        IsSelected = true;
        return this;
    }

    public ISelectionItem<T> AddChild(T item)
    {
        var node = new ListPromptItem<T>(item, this);
        Children.Add(node);
        return node;
    }

    public IEnumerable<ListPromptItem<T>> Traverse(bool includeSelf)
    {
        var stack = new Stack<ListPromptItem<T>>();

        if (includeSelf)
        {
            stack.Push(this);
        }
        else
        {
            foreach (var child in Children)
            {
                stack.Push(child);
            }
        }

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            yield return current;

            if (current.Children.Count > 0)
            {
                foreach (var child in current.Children.ReverseEnumerable())
                {
                    stack.Push(child);
                }
            }
        }
    }

    private static int CalculateDepth(ListPromptItem<T>? parent)
    {
        var level = 0;
        while (parent != null)
        {
            level++;
            parent = parent.Parent;
        }

        return level;
    }
}
