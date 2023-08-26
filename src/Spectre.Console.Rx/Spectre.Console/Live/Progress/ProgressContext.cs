// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using Spectre.Console.Rx;

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a context that can be used to interact with a <see cref="Progress"/>.
/// </summary>
public sealed class ProgressContext : IContext
{
    private readonly List<ProgressTask> _tasks;
    private readonly object _taskLock;
    private readonly IAnsiConsole _console;
    private readonly ProgressRenderer _renderer;
    private int _taskId;

    internal ProgressContext(IAnsiConsole console, ProgressRenderer renderer)
    {
        _tasks = new List<ProgressTask>();
        _taskLock = new object();
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    /// <summary>
    /// Gets a value indicating whether or not all started tasks have completed.
    /// </summary>
    public bool IsFinished => _tasks.Where(x => x.IsStarted).All(task => task.IsFinished);

    /// <summary>
    /// Adds a task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="autoStart">Whether or not the task should start immediately.</param>
    /// <param name="maxValue">The task's max value.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTask(string description, bool autoStart = true, double maxValue = 100) => AddTask(description, new ProgressTaskSettings
    {
        AutoStart = autoStart,
        MaxValue = maxValue,
    });

    /// <summary>
    /// Adds a task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="settings">The task settings.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTask(string description, ProgressTaskSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        lock (_taskLock)
        {
            var task = new ProgressTask(_taskId++, description, settings.MaxValue, settings.AutoStart);

            _tasks.Add(task);

            return task;
        }
    }

    /// <summary>
    /// Refreshes the current progress.
    /// </summary>
    public void Refresh()
    {
        _renderer.Update(this);
        _console.Write(new ControlCode(string.Empty));
    }

    /// <summary>
    /// Schedules the specified action until IsFinished is true.
    /// </summary>
    /// <param name="delay">The cyclic delay.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Schedule(TimeSpan delay, Action action)
    {
        var consoleScheduler = new SpectreConsoleScheduler();
        var lockTillComplete = new SemaphoreSlim(1);
        lockTillComplete.Wait();
        consoleScheduler.Schedule(async () =>
        {
            while (!IsFinished)
            {
                action();
                Refresh();
                await consoleScheduler.Sleep(delay);
            }

            lockTillComplete.Release();
        });

        lockTillComplete.Wait();
        lockTillComplete.Dispose();
        return Task.CompletedTask;
    }

    internal IReadOnlyList<ProgressTask> GetTasks()
    {
        lock (_taskLock)
        {
            return new List<ProgressTask>(_tasks);
        }
    }
}
