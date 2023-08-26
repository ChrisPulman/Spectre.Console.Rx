// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class DayOfWeekExtensions
{
    public static string GetAbbreviatedDayName(this DayOfWeek day, CultureInfo culture)
    {
        culture ??= CultureInfo.InvariantCulture;
        return culture.DateTimeFormat
            .GetAbbreviatedDayName(day)
            .CapitalizeFirstLetter(culture);
    }

    public static DayOfWeek GetNextWeekDay(this DayOfWeek day)
    {
        var next = (int)day + 1;
        if (next > (int)DayOfWeek.Saturday)
        {
            return DayOfWeek.Sunday;
        }

        return (DayOfWeek)next;
    }
}