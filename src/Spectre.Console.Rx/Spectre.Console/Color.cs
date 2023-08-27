// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a color.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Color"/> struct.
/// </remarks>
public partial struct Color(byte red, byte green, byte blue) : IEquatable<Color>
{
    static Color() => Default = new Color(0, 0, 0, 0, true);

    /// <summary>
    /// Gets the default color.
    /// </summary>
    public static Color Default { get; }

    /// <summary>
    /// Gets the red component.
    /// </summary>
    public byte R { get; } = red;

    /// <summary>
    /// Gets the green component.
    /// </summary>
    public byte G { get; } = green;

    /// <summary>
    /// Gets the blue component.
    /// </summary>
    public byte B { get; } = blue;

    /// <summary>
    /// Gets the number of the color, if any.
    /// </summary>
    internal byte? Number { get; }

    /// <summary>
    /// Gets a value indicating whether or not this is the default color.
    /// </summary>
    internal bool IsDefault { get; }

    /// <summary>
    /// Converts a <see cref="int"/> to a <see cref="Color"/>.
    /// </summary>
    /// <param name="number">The color number to convert.</param>
    public static implicit operator Color(int number) => FromInt32(number);

    /// <summary>
    /// Converts a <see cref="ConsoleColor"/> to a <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    public static implicit operator Color(ConsoleColor color) => FromConsoleColor(color);

    /// <summary>
    /// Converts a <see cref="Color"/> to a <see cref="ConsoleColor"/>.
    /// </summary>
    /// <param name="color">The console color to convert.</param>
    public static implicit operator ConsoleColor(Color color) => ToConsoleColor(color);

    /// <summary>
    /// Checks if two <see cref="Color"/> instances are equal.
    /// </summary>
    /// <param name="left">The first color instance to compare.</param>
    /// <param name="right">The second color instance to compare.</param>
    /// <returns><c>true</c> if the two colors are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(Color left, Color right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="Color"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first color instance to compare.</param>
    /// <param name="right">The second color instance to compare.</param>
    /// <returns><c>true</c> if the two colors are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Color left, Color right) => !(left == right);

    /// <summary>
    /// Converts a <see cref="Color"/> to a <see cref="ConsoleColor"/>.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>A <see cref="ConsoleColor"/> representing the <see cref="Color"/>.</returns>
    public static ConsoleColor ToConsoleColor(Color color)
    {
        if (color.IsDefault)
        {
            return (ConsoleColor)(-1);
        }

        if (color.Number == null || color.Number.Value >= 16)
        {
            color = ColorPalette.ExactOrClosest(ColorSystem.Standard, color);
        }

        // Should not happen, but this will make things easier if we mess things up...
        Debug.Assert(
            color.Number >= 0 && color.Number < 16,
            "Color does not fall inside the standard palette range.");

        return color.Number.Value switch
        {
            0 => ConsoleColor.Black,
            1 => ConsoleColor.DarkRed,
            2 => ConsoleColor.DarkGreen,
            3 => ConsoleColor.DarkYellow,
            4 => ConsoleColor.DarkBlue,
            5 => ConsoleColor.DarkMagenta,
            6 => ConsoleColor.DarkCyan,
            7 => ConsoleColor.Gray,
            8 => ConsoleColor.DarkGray,
            9 => ConsoleColor.Red,
            10 => ConsoleColor.Green,
            11 => ConsoleColor.Yellow,
            12 => ConsoleColor.Blue,
            13 => ConsoleColor.Magenta,
            14 => ConsoleColor.Cyan,
            15 => ConsoleColor.White,
            _ => throw new InvalidOperationException("Cannot convert color to console color."),
        };
    }

    /// <summary>
    /// Converts a color number into a <see cref="Color"/>.
    /// </summary>
    /// <param name="number">The color number.</param>
    /// <returns>The color representing the specified color number.</returns>
    public static Color FromInt32(int number) => ColorTable.GetColor(number);

    /// <summary>
    /// Converts a <see cref="ConsoleColor"/> to a <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>A <see cref="Color"/> representing the <see cref="ConsoleColor"/>.</returns>
    public static Color FromConsoleColor(ConsoleColor color) => color switch
    {
        ConsoleColor.Black => Black,
        ConsoleColor.Blue => Blue,
        ConsoleColor.Cyan => Aqua,
        ConsoleColor.DarkBlue => Navy,
        ConsoleColor.DarkCyan => Teal,
        ConsoleColor.DarkGray => Grey,
        ConsoleColor.DarkGreen => Green,
        ConsoleColor.DarkMagenta => Purple,
        ConsoleColor.DarkRed => Maroon,
        ConsoleColor.DarkYellow => Olive,
        ConsoleColor.Gray => Silver,
        ConsoleColor.Green => Lime,
        ConsoleColor.Magenta => Fuchsia,
        ConsoleColor.Red => Red,
        ConsoleColor.White => White,
        ConsoleColor.Yellow => Yellow,
        _ => Default,
    };

    /// <summary>
    /// Blends two colors.
    /// </summary>
    /// <param name="other">The other color.</param>
    /// <param name="factor">The blend factor.</param>
    /// <returns>The resulting color.</returns>
    public readonly Color Blend(Color other, float factor) =>
        new(
            (byte)(R + ((other.R - R) * factor)),
            (byte)(G + ((other.G - G) * factor)),
            (byte)(B + ((other.B - B) * factor)));

    /// <summary>
    /// Gets the hexadecimal representation of the color.
    /// </summary>
    /// <returns>The hexadecimal representation of the color.</returns>
    public readonly string ToHex() => string.Format(
            CultureInfo.InvariantCulture,
            "{0}{1}{2}",
            R.ToString("X2", CultureInfo.InvariantCulture),
            G.ToString("X2", CultureInfo.InvariantCulture),
            B.ToString("X2", CultureInfo.InvariantCulture));

    /// <inheritdoc/>
    public override readonly int GetHashCode()
    {
#if NETSTANDARD2_0
        unchecked
        {
            var hash = (int)2166136261;
            hash = (hash * 16777619) ^ R.GetHashCode();
            hash = (hash * 16777619) ^ G.GetHashCode();
            hash = (hash * 16777619) ^ B.GetHashCode();
            return hash;
        }
#else
#pragma warning disable IDE0022 // Use expression body for method
        return HashCode.Combine(R, G, B);
#pragma warning restore IDE0022 // Use expression body for method
#endif
    }

    /// <inheritdoc/>
    public override readonly bool Equals(object? obj) => obj is Color color && Equals(color);

    /// <inheritdoc/>
    public readonly bool Equals(Color other) => (IsDefault && other.IsDefault) ||
               (IsDefault == other.IsDefault && R == other.R && G == other.G && B == other.B);

    /// <summary>
    /// Converts the color to a markup string.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the color as markup.</returns>
    public readonly string ToMarkup()
    {
        if (IsDefault)
        {
            return "default";
        }

        if (Number != null)
        {
            var name = ColorTable.GetName(Number.Value);
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
        }

        return string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", R, G, B);
    }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        if (IsDefault)
        {
            return "default";
        }

        if (Number != null)
        {
            var name = ColorTable.GetName(Number.Value);
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
        }

        return string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2} (RGB={0},{1},{2})", R, G, B);
    }
}
