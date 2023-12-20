// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class ShouldlyExtensions
{
    [DebuggerStepThrough]
    public static T And<T>(this T item, Action<T> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        action(item);
        return item;
    }
}
