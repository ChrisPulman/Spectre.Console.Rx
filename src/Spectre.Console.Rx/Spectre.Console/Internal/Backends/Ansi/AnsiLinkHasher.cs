// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class AnsiLinkHasher
{
    private readonly Random _random;

    public AnsiLinkHasher() => _random = new Random(Environment.TickCount);

    public int GenerateId(string link, string text)
    {
        if (link is null)
        {
            throw new ArgumentNullException(nameof(link));
        }

        link += text ?? string.Empty;

        unchecked
        {
            return Math.Abs(
                GetLinkHashCode(link) +
                _random.Next(0, int.MaxValue));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetLinkHashCode(string link) =>
#if NETSTANDARD2_0
        link.GetHashCode();
#else
        link.GetHashCode(StringComparison.Ordinal);
#endif
}
