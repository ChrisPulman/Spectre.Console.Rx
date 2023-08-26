// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Internal;

internal static partial class CharExtensions
{
    public static bool IsDigit(this char character, int min = 0) => char.IsDigit(character) && character >= (char)min;
}
