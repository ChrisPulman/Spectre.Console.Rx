// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class RenderOptionsExtensions
{
    public static BoxBorder GetSafeBorder<T>(this RenderOptions options, T border)
        where T : IHasBoxBorder, IHasBorder => border.Border.GetSafeBorder(!options.Unicode && border.UseSafeBorder);
}
