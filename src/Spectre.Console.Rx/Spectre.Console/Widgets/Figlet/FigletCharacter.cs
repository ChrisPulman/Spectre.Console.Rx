// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class FigletCharacter
{
    public FigletCharacter(int code, IEnumerable<string> lines)
    {
        Code = code;
        Lines = new List<string>(lines ?? throw new ArgumentNullException(nameof(lines)));

        var min = Lines.Min(x => x.Length);
        var max = Lines.Max(x => x.Length);
        if (min != max)
        {
            throw new InvalidOperationException($"Figlet character #{code} has varying width");
        }

        Width = max;
        Height = Lines.Count;
    }

    public int Code { get; }

    public int Width { get; }

    public int Height { get; }

    public IReadOnlyList<string> Lines { get; }
}
