// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.ReactiveUI;

/// <summary>
/// ReactiveUI builder extensions for Spectre.Console.Rx applications.
/// </summary>
public static class SpectreConsoleRxReactiveUIBuilderExtensions
{
    /// <summary>
    /// Configures ReactiveUI for a Spectre.Console.Rx console application.
    /// </summary>
    /// <param name="builder">The ReactiveUI builder.</param>
    /// <param name="mainThreadScheduler">The main-thread scheduler. Defaults to <see cref="AnsiConsoleRx.Scheduler"/>.</param>
    /// <param name="taskPoolScheduler">The task-pool scheduler. Defaults to <see cref="global::System.Reactive.Concurrency.TaskPoolScheduler.Default"/>.</param>
    /// <param name="registerPlatformServices">Whether to register ReactiveUI's base .NET platform services.</param>
    /// <returns>The same builder for chaining.</returns>
    public static global::ReactiveUI.Builder.IReactiveUIBuilder WithSpectreConsoleRx(
        this global::ReactiveUI.Builder.IReactiveUIBuilder builder,
        global::System.Reactive.Concurrency.IScheduler? mainThreadScheduler = null,
        global::System.Reactive.Concurrency.IScheduler? taskPoolScheduler = null,
        bool registerPlatformServices = true)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.WithCoreServices();
        builder
            .WithTaskPoolScheduler(taskPoolScheduler ?? global::System.Reactive.Concurrency.TaskPoolScheduler.Default)
            .WithMainThreadScheduler(mainThreadScheduler ?? AnsiConsoleRx.Scheduler);

        if (registerPlatformServices)
        {
            builder.WithPlatformServices();
        }

        return builder;
    }
}
