// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class ConsoleHelper
{
    public static int GetSafeWidth(int defaultValue = Constants.DefaultTerminalWidth)
    {
        try
        {
            var width = System.Console.BufferWidth;
            if (width == 0)
            {
                width = defaultValue;
            }

            return width;
        }
        catch (IOException)
        {
            return defaultValue;
        }
    }

    public static int GetSafeHeight(int defaultValue = Constants.DefaultTerminalHeight)
    {
        try
        {
            var height = System.Console.WindowHeight;
            if (height == 0)
            {
                height = defaultValue;
            }

            return height;
        }
        catch (IOException)
        {
            return defaultValue;
        }
    }
}