// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a calendar event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CalendarEvent"/> class.
/// </remarks>
/// <param name="description">The calendar event description.</param>
/// <param name="year">The year of the calendar event.</param>
/// <param name="month">The month of the calendar event.</param>
/// <param name="day">The day of the calendar event.</param>
public sealed class CalendarEvent(string description, int year, int month, int day)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarEvent"/> class.
    /// </summary>
    /// <param name="year">The year of the calendar event.</param>
    /// <param name="month">The month of the calendar event.</param>
    /// <param name="day">The day of the calendar event.</param>
    public CalendarEvent(int year, int month, int day)
        : this(string.Empty, year, month, day)
    {
    }

    /// <summary>
    /// Gets the description of the calendar event.
    /// </summary>
    public string Description { get; } = description ?? string.Empty;

    /// <summary>
    /// Gets the year of the calendar event.
    /// </summary>
    public int Year { get; } = year;

    /// <summary>
    /// Gets the month of the calendar event.
    /// </summary>
    public int Month { get; } = month;

    /// <summary>
    /// Gets the day of the calendar event.
    /// </summary>
    public int Day { get; } = day;
}
