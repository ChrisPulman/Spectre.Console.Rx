// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Internal;

internal sealed class TextEncoder : IAnsiConsoleEncoder
{
    public string Encode(IAnsiConsole console, IEnumerable<IRenderable> renderables)
    {
        var context = RenderOptions.Create(console, new EncoderCapabilities(ColorSystem.TrueColor));
        var builder = new StringBuilder();

        foreach (var renderable in renderables)
        {
            var segments = renderable.Render(context, console.Profile.Width);
            foreach (var segment in Segment.Merge(segments))
            {
                if (segment.IsControlCode)
                {
                    continue;
                }

                builder.Append(segment.Text);
            }
        }

        return builder.ToString().TrimEnd('\n');
    }
}