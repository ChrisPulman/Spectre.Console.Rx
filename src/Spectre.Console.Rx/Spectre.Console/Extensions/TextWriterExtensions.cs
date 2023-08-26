// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class TextWriterExtensions
{
    public static bool IsStandardOut(this TextWriter writer)
    {
        try
        {
            return writer == System.Console.Out;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsStandardError(this TextWriter writer)
    {
        try
        {
            return writer == System.Console.Error;
        }
        catch
        {
            return false;
        }
    }
}