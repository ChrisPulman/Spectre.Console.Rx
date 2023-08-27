// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendWithStyle(this StringBuilder builder, Style? style, int? value) => AppendWithStyle(builder, style, value?.ToString(CultureInfo.InvariantCulture));

    public static StringBuilder AppendWithStyle(this StringBuilder builder, Style? style, string? value)
    {
        value ??= string.Empty;

        if (style != null)
        {
            return builder.Append('[')
            .Append(style.ToMarkup())
            .Append(']')
            .Append(value.EscapeMarkup())
            .Append("[/]");
        }

        return builder.Append(value);
    }

    public static void AppendSpan(this StringBuilder builder, in ReadOnlySpan<char> span) =>

        // NetStandard 2 lacks the override for StringBuilder to add the span. We'll need to convert the span
        // to a string for it, but for .NET 6.0 or newer we'll use the override.
#if NETSTANDARD2_0
        builder.Append(span.ToString());
#else
        builder.Append(span);
#endif

}
