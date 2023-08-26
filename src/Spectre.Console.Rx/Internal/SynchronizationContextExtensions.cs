// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Internal;

internal static class SynchronizationContextExtensions
{
    public static void PostWithStartComplete<T>(this SynchronizationContext context, Action<T> action, T state)
    {
        context.OperationStarted();

        context.Post(
            o =>
            {
                try
                {
                    action((T)o!);
                }
                finally
                {
                    context.OperationCompleted();
                }
            },
            state);
    }

    public static void PostWithStartComplete(this SynchronizationContext context, Action action)
    {
        context.OperationStarted();

        context.Post(
            () =>
            {
                try
                {
                    action();
                }
                finally
                {
                    context.OperationCompleted();
                }
            });
    }

    public static void Post(this SynchronizationContext context, Action action) => context.Post(_ => action(), null);
}
