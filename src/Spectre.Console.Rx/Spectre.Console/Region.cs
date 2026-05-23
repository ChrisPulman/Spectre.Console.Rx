namespace Spectre.Console.Rx;

/// <summary>
/// Represents a region.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Region"/> struct.
/// </remarks>
/// <param name="x">The x-coordinate.</param>
/// <param name="y">The y-coordinate.</param>
/// <param name="width">The width.</param>
/// <param name="height">The height.</param>
[DebuggerDisplay("[X={X,nq}, Y={Y,nq}, W={Width,nq}, H={Height,nq}]")]
public readonly struct Region(int x, int y, int width, int height)
{
    /// <summary>
    /// Gets the x-coordinate.
    /// </summary>
    public int X { get; } = x;

    /// <summary>
    /// Gets the y-coordinate.
    /// </summary>
    public int Y { get; } = y;

    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; } = width;

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; } = height;
}