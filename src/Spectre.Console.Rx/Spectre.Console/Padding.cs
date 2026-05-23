namespace Spectre.Console.Rx;

/// <summary>
/// Represents padding.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Padding"/> struct.
/// </remarks>
/// <param name="left">The left padding.</param>
/// <param name="top">The top padding.</param>
/// <param name="right">The right padding.</param>
/// <param name="bottom">The bottom padding.</param>
public readonly struct Padding(int left, int top, int right, int bottom) : IEquatable<Padding>
{
    /// <summary>
    /// Gets the left padding.
    /// </summary>
    public int Left { get; } = left;

    /// <summary>
    /// Gets the top padding.
    /// </summary>
    public int Top { get; } = top;

    /// <summary>
    /// Gets the right padding.
    /// </summary>
    public int Right { get; } = right;

    /// <summary>
    /// Gets the bottom padding.
    /// </summary>
    public int Bottom { get; } = bottom;

    /// <summary>
    /// Initializes a new instance of the <see cref="Padding"/> struct.
    /// </summary>
    /// <param name="size">The padding for all sides.</param>
    public Padding(int size)
        : this(size, size, size, size)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Padding"/> struct.
    /// </summary>
    /// <param name="horizontal">The left and right padding.</param>
    /// <param name="vertical">The top and bottom padding.</param>
    public Padding(int horizontal, int vertical)
        : this(horizontal, vertical, horizontal, vertical)
    {
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Padding padding && Equals(padding);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = (int)2166136261;
            hash = (hash * 16777619) ^ Left.GetHashCode();
            hash = (hash * 16777619) ^ Top.GetHashCode();
            hash = (hash * 16777619) ^ Right.GetHashCode();
            hash = (hash * 16777619) ^ Bottom.GetHashCode();
            return hash;
        }
    }

    /// <inheritdoc/>
    public bool Equals(Padding other) => Left == other.Left
            && Top == other.Top
            && Right == other.Right
            && Bottom == other.Bottom;

    /// <summary>
    /// Checks if two <see cref="Padding"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Padding"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Padding"/> instance to compare.</param>
    /// <returns><c>true</c> if the two instances are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(Padding left, Padding right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="Padding"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Padding"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Padding"/> instance to compare.</param>
    /// <returns><c>true</c> if the two instances are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Padding left, Padding right) => !(left == right);

    /// <summary>
    /// Gets the padding width.
    /// </summary>
    /// <returns>The padding width.</returns>
    public int GetWidth() => Left + Right;

    /// <summary>
    /// Gets the padding height.
    /// </summary>
    /// <returns>The padding height.</returns>
    public int GetHeight() => Top + Bottom;
}

/// <summary>
/// Contains extension methods for <see cref="Padding"/>.
/// </summary>
public static class PaddingExtensions
{
    /// <summary>
    /// Gets the left padding.
    /// </summary>
    /// <param name="padding">The padding.</param>
    /// <returns>The left padding or zero if <c>padding</c> is null.</returns>
    public static int GetLeftSafe(this Padding? padding) => padding?.Left ?? 0;

    /// <summary>
    /// Gets the right padding.
    /// </summary>
    /// <param name="padding">The padding.</param>
    /// <returns>The right padding or zero if <c>padding</c> is null.</returns>
    public static int GetRightSafe(this Padding? padding) => padding?.Right ?? 0;

    /// <summary>
    /// Gets the top padding.
    /// </summary>
    /// <param name="padding">The padding.</param>
    /// <returns>The top padding or zero if <c>padding</c> is null.</returns>
    public static int GetTopSafe(this Padding? padding) => padding?.Top ?? 0;

    /// <summary>
    /// Gets the bottom padding.
    /// </summary>
    /// <param name="padding">The padding.</param>
    /// <returns>The bottom padding or zero if <c>padding</c> is null.</returns>
    public static int GetBottomSafe(this Padding? padding) => padding?.Bottom ?? 0;
}