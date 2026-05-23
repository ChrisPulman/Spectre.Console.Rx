namespace Spectre.Console.Rx;

/// <summary>
/// Represents a size.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Size"/> struct.
/// </remarks>
/// <param name="width">The width.</param>
/// <param name="height">The height.</param>
[DebuggerDisplay("{Width,nq}x{Height,nq}")]
public readonly struct Size(int width, int height)
{
    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; } = width;

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; } = height;
}