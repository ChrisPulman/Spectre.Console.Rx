// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Linq;

namespace Spectre.Console.Rx;

internal sealed class ProgressRefreshThread(ProgressContext context, TimeSpan refreshRate) : IDisposable
{
    private readonly IDisposable _subscription = Observable.Interval(refreshRate)
            .ObserveOn(AnsiConsoleRx.Scheduler)
            .Subscribe(_ => context.Refresh());

    public void Dispose() => _subscription.Dispose();
}
