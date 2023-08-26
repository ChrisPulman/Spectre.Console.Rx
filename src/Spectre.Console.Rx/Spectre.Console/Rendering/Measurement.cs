// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Rendering;

/// <summary>
/// Represents a measurement.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Measurement"/> struct.
/// </remarks>
/// <param name="min">The minimum width.</param>
/// <param name="max">The maximum width.</param>
public readonly struct Measurement(int min, int max) : IEquatable<Measurement>
{
    /// <summary>
    /// Gets the minimum width.
    /// </summary>
    public int Min { get; } = min;

    /// <summary>
    /// Gets the maximum width.
    /// </summary>
    public int Max { get; } = max;

    /// <summary>
    /// Checks if two <see cref="Measurement"/> instances are equal.
    /// </summary>
    /// <param name="left">The first measurement instance to compare.</param>
    /// <param name="right">The second measurement instance to compare.</param>
    /// <returns><c>true</c> if the two measurements are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(Measurement left, Measurement right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="Measurement"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first measurement instance to compare.</param>
    /// <param name="right">The second measurement instance to compare.</param>
    /// <returns><c>true</c> if the two measurements are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Measurement left, Measurement right) => !(left == right);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Measurement measurement && Equals(measurement);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = (int)2166136261;
            hash = (hash * 16777619) ^ Min.GetHashCode();
            hash = (hash * 16777619) ^ Max.GetHashCode();
            return hash;
        }
    }

    /// <inheritdoc/>
    public bool Equals(Measurement other) => Min == other.Min && Max == other.Max;
}
