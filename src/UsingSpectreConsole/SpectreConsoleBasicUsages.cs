// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Spectre.Console.Rx;
using Spectre.Console.Rx.Json;

namespace UsingSpectreDotConsole;

/// <summary>
/// SpectreConsoleBasicUsages.
/// </summary>
public static class SpectreConsoleBasicUsages
{
    private static readonly List<Student> _students = StudentsGenerator.GenerateStudents();

    /// <summary>
    /// Sets the color of the text.
    /// </summary>
    public static void SetTextColor()
    {
        AnsiConsole.Markup($"[bold blue]Hello[/] [italic green]{_students[1].FirstName}[/]!");
        AnsiConsole.Write(new Markup($"[underline #800080]{_students[2].FirstName}[/]"));
        AnsiConsole.MarkupLine($"[rgb(128,0,0)]{_students[3].FirstName}[/]");
    }

    /// <summary>
    /// Sets the color of the background.
    /// </summary>
    public static void SetBackgroundColor() =>
        AnsiConsole.MarkupLine($"[red on yellow] Hello, {_students[4].LastName}![/]");

    /// <summary>
    /// Escapes the format characters.
    /// </summary>
    public static void EscapeFormatCharacters()
    {
        AnsiConsole.Markup($"[[{_students[3].FirstName}]]");
        AnsiConsole.MarkupLine($"[blue][[{_students[3].Hostel}]][/]");
        AnsiConsole.MarkupLine($"[{_students[3].Age}]".EscapeMarkup());
    }

    /// <summary>
    /// Beautifies the students data json.
    /// </summary>
    /// <returns>A Panel.</returns>
    public static Panel BeautifyStudentsDataJson()
    {
        var json = new JsonText(StudentsGenerator.ConvertStudentsToJson(_students));
        var panel = new Panel(json)
                .Header("Students")
                .HeaderAlignment(Justify.Center)
                .SquareBorder()
                .Collapse()
                .BorderColor(Color.LightSkyBlue1);

        AnsiConsole.Write(panel);

        return panel;
    }

    /// <summary>
    /// Pretties the print calendar.
    /// </summary>
    /// <returns>A Calendar.</returns>
    public static Calendar PrettyPrintCalendar()
    {
        var calendar = new Calendar(2023, 11)
            .AddCalendarEvent(2023, 11, 19)
            .HighlightStyle(Style.Parse("magenta bold"))
            .HeaderStyle(Style.Parse("purple"));

        AnsiConsole.Write(calendar);

        return calendar;
    }
}
