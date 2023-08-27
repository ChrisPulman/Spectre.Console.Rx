// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Contains extension methods for <see cref="Calendar"/>.
/// </summary>
public static class CalendarExtensions
{
    /// <summary>
    /// Adds a calendar event.
    /// </summary>
    /// <param name="calendar">The calendar to add the calendar event to.</param>
    /// <param name="date">The calendar event date.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
#if NET6_0_OR_GREATER
    [SuppressMessage("Roslynator", "RCS1231:Make parameter ref read-only.", Justification = "Netstandard does not support")]
#endif
    public static Calendar AddCalendarEvent(this Calendar calendar, DateTime date) => AddCalendarEvent(calendar, string.Empty, date.Year, date.Month, date.Day);

    /// <summary>
    /// Adds a calendar event.
    /// </summary>
    /// <param name="calendar">The calendar to add the calendar event to.</param>
    /// <param name="description">The calendar event description.</param>
    /// <param name="date">The calendar event date.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
#if NET6_0_OR_GREATER
    [SuppressMessage("Roslynator", "RCS1231:Make parameter ref read-only.", Justification = "Netstandard does not support")]
#endif
    public static Calendar AddCalendarEvent(this Calendar calendar, string description, DateTime date) => AddCalendarEvent(calendar, description, date.Year, date.Month, date.Day);

    /// <summary>
    /// Adds a calendar event.
    /// </summary>
    /// <param name="calendar">The calendar to add the calendar event to.</param>
    /// <param name="year">The year of the calendar event.</param>
    /// <param name="month">The month of the calendar event.</param>
    /// <param name="day">The day of the calendar event.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Calendar AddCalendarEvent(this Calendar calendar, int year, int month, int day) => AddCalendarEvent(calendar, string.Empty, year, month, day);

    /// <summary>
    /// Adds a calendar event.
    /// </summary>
    /// <param name="calendar">The calendar.</param>
    /// <param name="description">The calendar event description.</param>
    /// <param name="year">The year of the calendar event.</param>
    /// <param name="month">The month of the calendar event.</param>
    /// <param name="day">The day of the calendar event.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Calendar AddCalendarEvent(this Calendar calendar, string description, int year, int month, int day)
    {
        if (calendar is null)
        {
            throw new ArgumentNullException(nameof(calendar));
        }

        calendar.CalendarEvents.Add(new CalendarEvent(description, year, month, day));
        return calendar;
    }

    /// <summary>
    /// Sets the calendar's highlight <see cref="Style"/>.
    /// </summary>
    /// <param name="calendar">The calendar.</param>
    /// <param name="style">The highlight style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Calendar HighlightStyle(this Calendar calendar, Style? style)
    {
        if (calendar is null)
        {
            throw new ArgumentNullException(nameof(calendar));
        }

        calendar.HighlightStyle = style ?? Style.Plain;
        return calendar;
    }

    /// <summary>
    /// Sets the calendar's header <see cref="Style"/>.
    /// </summary>
    /// <param name="calendar">The calendar.</param>
    /// <param name="style">The header style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Calendar HeaderStyle(this Calendar calendar, Style? style)
    {
        if (calendar is null)
        {
            throw new ArgumentNullException(nameof(calendar));
        }

        calendar.HeaderStyle = style ?? Style.Plain;
        return calendar;
    }

    /// <summary>
    /// Shows the calendar header.
    /// </summary>
    /// <param name="calendar">The calendar.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Calendar ShowHeader(this Calendar calendar)
    {
        if (calendar is null)
        {
            throw new ArgumentNullException(nameof(calendar));
        }

        calendar.ShowHeader = true;
        return calendar;
    }

    /// <summary>
    /// Hides the calendar header.
    /// </summary>
    /// <param name="calendar">The calendar.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Calendar HideHeader(this Calendar calendar)
    {
        if (calendar is null)
        {
            throw new ArgumentNullException(nameof(calendar));
        }

        calendar.ShowHeader = false;
        return calendar;
    }
}
