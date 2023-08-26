// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ProgressDemo;

/// <summary>
/// DescriptionGenerator.
/// </summary>
public static class DescriptionGenerator
{
    private static readonly string[] _verbs = new[] { "Downloading", "Rerouting", "Retriculating", "Collapsing", "Folding", "Solving", "Colliding", "Measuring" };
    private static readonly string[] _nouns = new[] { "internet", "splines", "space", "capacitators", "quarks", "algorithms", "data structures", "spacetime" };

    private static readonly Random _random;
    private static readonly HashSet<string> _used;

    static DescriptionGenerator()
    {
        _random = new Random(DateTime.Now.Millisecond);
        _used = new HashSet<string>();
    }

    /// <summary>
    /// Tries the generate.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>A bool.</returns>
    public static bool TryGenerate(out string name)
    {
        for (var iterations = 0; iterations < 25; iterations++)
        {
            name = Generate();
            if (!_used.Contains(name))
            {
                _used.Add(name);
                return true;
            }
        }

        name = Generate();
        return false;
    }

    /// <summary>
    /// Generates this instance.
    /// </summary>
    /// <returns>A string.</returns>
    public static string Generate() =>
        _verbs[_random.Next(0, _verbs.Length)]
        + " "
        + _nouns[_random.Next(0, _nouns.Length)];
}
