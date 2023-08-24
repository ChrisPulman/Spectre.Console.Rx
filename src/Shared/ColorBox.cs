// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console.Examples;

/// <summary>
/// ColorBox.
/// </summary>
/// <seealso cref="Spectre.Console.Rendering.Renderable" />
public sealed class ColorBox : Renderable
{
    private readonly int _height;
    private readonly int? _width;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorBox"/> class.
    /// </summary>
    /// <param name="height">The height.</param>
    public ColorBox(int height)
    {
        _height = height;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorBox"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public ColorBox(int width, int height)
        : this(height)
    {
        _width = width;
    }

    /// <summary>
    /// Measures the renderable object.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="maxWidth">The maximum allowed width.</param>
    /// <returns>
    /// The minimum and maximum width of the object.
    /// </returns>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        return new Measurement(1, GetWidth(maxWidth));
    }

    /// <summary>
    /// Renders the object.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="maxWidth">The maximum allowed width.</param>
    /// <returns>
    /// A collection of segments.
    /// </returns>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        maxWidth = GetWidth(maxWidth);

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < maxWidth; x++)
            {
                var h = x / (float)maxWidth;
                var l = 0.1f + ((y / (float)_height) * 0.7f);
                var (r1, g1, b1) = ColorFromHSL(h, l, 1.0f);
                var (r2, g2, b2) = ColorFromHSL(h, l + (0.7f / 10), 1.0f);

                var background = new Color((byte)(r1 * 255), (byte)(g1 * 255), (byte)(b1 * 255));
                var foreground = new Color((byte)(r2 * 255), (byte)(g2 * 255), (byte)(b2 * 255));

                yield return new Segment("▄", new Style(foreground, background));
            }

            yield return Segment.LineBreak;
        }
    }

    private static (float, float, float) ColorFromHSL(double h, double l, double s)
    {
        double r = 0, g = 0, b = 0;
        if (l != 0)
        {
            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                double temp2;
                if (l < 0.5)
                {
                    temp2 = l * (1.0 + s);
                }
                else
                {
                    temp2 = l + s - (l * s);
                }

                var temp1 = (2.0 * l) - temp2;

                r = GetColorComponent(temp1, temp2, h + (1.0 / 3.0));
                g = GetColorComponent(temp1, temp2, h);
                b = GetColorComponent(temp1, temp2, h - (1.0 / 3.0));
            }
        }

        return ((float)r, (float)g, (float)b);
    }

    private static double GetColorComponent(double temp1, double temp2, double temp3)
    {
        if (temp3 < 0.0)
        {
            temp3 += 1.0;
        }
        else if (temp3 > 1.0)
        {
            temp3 -= 1.0;
        }

        if (temp3 < 1.0 / 6.0)
        {
            return temp1 + ((temp2 - temp1) * 6.0 * temp3);
        }
        else if (temp3 < 0.5)
        {
            return temp2;
        }
        else if (temp3 < 2.0 / 3.0)
        {
            return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
        }
        else
        {
            return temp1;
        }
    }

    private int GetWidth(int maxWidth)
    {
        var width = maxWidth;
        if (_width != null)
        {
            width = Math.Min(_width.Value, width);
        }

        return width;
    }
}
