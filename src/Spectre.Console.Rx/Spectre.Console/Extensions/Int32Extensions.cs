// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class Int32Extensions
{
    public static int Clamp(this int value, int min, int max)
    {
        if (value <= min)
        {
            return min;
        }

        if (value >= max)
        {
            return max;
        }

        return value;
    }
}