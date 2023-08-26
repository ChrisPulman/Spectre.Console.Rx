// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class FallbackStatusRenderer : ProgressRenderer
{
    private readonly object _lock;
    private IRenderable? _renderable;
    private string? _lastStatus;

    public FallbackStatusRenderer() => _lock = new object();

    public override TimeSpan RefreshRate => TimeSpan.FromMilliseconds(100);

    public override void Update(ProgressContext context)
    {
        lock (_lock)
        {
            var task = context.GetTasks().SingleOrDefault();
            if (task != null)
            {
                // Not same description?
                if (_lastStatus != task.Description)
                {
                    _lastStatus = task.Description;
                    _renderable = new Markup(task.Description + Environment.NewLine);
                    return;
                }
            }

            _renderable = null;
            return;
        }
    }

    public override IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables)
    {
        lock (_lock)
        {
            var result = new List<IRenderable>();
            result.AddRange(renderables);

            if (_renderable != null)
            {
                result.Add(_renderable);
            }

            _renderable = null;

            return result;
        }
    }
}
