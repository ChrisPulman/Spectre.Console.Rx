// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal abstract class ProgressRenderer : IRenderHook
{
    public abstract TimeSpan RefreshRate { get; }

    public virtual void Started()
    {
    }

    public virtual void Completed(bool clear)
    {
    }

    public abstract void Update(ProgressContext context);

    public abstract IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables);
}