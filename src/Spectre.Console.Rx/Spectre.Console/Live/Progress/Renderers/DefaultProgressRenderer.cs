// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class DefaultProgressRenderer(IAnsiConsole console, List<ProgressColumn> columns, TimeSpan refreshRate, bool hideCompleted, Func<IRenderable, IReadOnlyList<ProgressTask>, IRenderable> renderHook) : ProgressRenderer
{
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));
    private readonly List<ProgressColumn> _columns = columns ?? throw new ArgumentNullException(nameof(columns));
    private readonly LiveRenderable _live = new(console);
    private readonly object _lock = new();
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _lastUpdate = TimeSpan.Zero;

    public override TimeSpan RefreshRate { get; } = refreshRate;

    public override void Started() => _console.Cursor.Hide();

    public override void Completed(bool clear)
    {
        lock (_lock)
        {
            if (clear)
            {
                _console.Write(_live.RestoreCursor());
            }
            else
            {
                if (_live.HasRenderable && _live.DidOverflow)
                {
                    // Redraw the whole live renderable
                    _console.Write(_live.RestoreCursor());
                    _live.Overflow = VerticalOverflow.Visible;
                    _console.Write(_live.Target);
                }

                _console.WriteLine();
            }

            _console.Cursor.Show();
        }
    }

    public override void Update(ProgressContext context)
    {
        lock (_lock)
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
            }

            var renderContext = RenderOptions.Create(_console, _console.Profile.Capabilities);

            var delta = _stopwatch.Elapsed - _lastUpdate;
            _lastUpdate = _stopwatch.Elapsed;

            var grid = new Grid();
            for (var columnIndex = 0; columnIndex < _columns.Count; columnIndex++)
            {
                var column = new GridColumn().PadRight(1);

                var columnWidth = _columns[columnIndex].GetColumnWidth(renderContext);
                if (columnWidth != null)
                {
                    column.Width = columnWidth;
                }

                if (_columns[columnIndex].NoWrap)
                {
                    column.NoWrap();
                }

                // Last column?
                if (columnIndex == _columns.Count - 1)
                {
                    column.PadRight(0);
                }

                grid.AddColumn(column);
            }

            // Add rows
            var tasks = context.GetTasks();

            var layout = new Grid();
            layout.AddColumn();

            foreach (var task in tasks.Where(tsk => !(hideCompleted && tsk.IsFinished)))
            {
                var columns = _columns.Select(column => column.Render(renderContext, task, delta));
                grid.AddRow(columns.ToArray());
            }

            layout.AddRow(grid);

            _live.SetRenderable(new Padder(renderHook(layout, tasks), new Padding(0, 1)));
        }
    }

    public override IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables)
    {
        lock (_lock)
        {
            yield return _live.PositionCursor();

            foreach (var renderable in renderables)
            {
                yield return renderable;
            }

            yield return _live;
        }
    }
}
