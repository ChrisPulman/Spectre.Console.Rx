// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Spectre.Console.Rx;

namespace UsingSpectreDotConsole;

/// <summary>
/// SpectreConsoleOtherUsages.
/// </summary>
public static class SpectreConsoleOtherUsages
{
    private static readonly List<Student> _students = StudentsGenerator.GenerateStudents();

    /// <summary>
    /// Creates the student table.
    /// </summary>
    /// <returns>A Table.</returns>
    public static Table CreateStudentTable()
    {
        var table = new Table
        {
            Title = new TableTitle("STUDENTS", "bold green")
        };

        table.AddColumns("[yellow]Id[/]", $"[{Color.Olive}]FirstName[/]", "[Fuchsia]Age[/]");

        foreach (var student in _students)
        {
            table.AddRow(student.Id.ToString(), $"[red]{student.FirstName}[/]", $"[cyan]{student.Age}[/]");
        }

        AnsiConsole.Write(table);

        return table;
    }

    /// <summary>
    /// Writes the readable exception.
    /// </summary>
    public static void WriteReadableException()
    {
        try
        {
            File.OpenRead("nofile.txt");
        }
        catch (FileNotFoundException ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenMethods);
        }
    }

    /// <summary>
    /// Gets the age text prompt.
    /// </summary>
    /// <returns>A TextPrompt.</returns>
    public static TextPrompt<int> GetAgeTextPrompt() =>
        new TextPrompt<int>("[green]How old are you[/]?")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]That's not a valid age[/]")
                .Validate(age => age switch
                    {
                        <= 10 => ValidationResult.Error("[red]You must be above 10 years[/]"),
                        >= 123 => ValidationResult.Error("[red]You must be younger than that[/]"),
                        _ => ValidationResult.Success(),
                    });

    /// <summary>
    /// Gets the hostel selection prompt.
    /// </summary>
    /// <returns>A SelectionPrompt.</returns>
    public static SelectionPrompt<string> GetHostelSelectionPrompt()
    {
        var hostels = StudentsGenerator.Hostels;

        return new SelectionPrompt<string>()
                 .Title("Choose a hostel")
                 .AddChoices(hostels);
    }

    /// <summary>
    /// Prompts the student.
    /// </summary>
    /// <returns>A Student.</returns>
    public static Student PromptStudent()
    {
        var student = new Student
        {
            FirstName = AnsiConsole.Ask<string>("[green]What's your First Name[/]?"),
            LastName = AnsiConsole.Ask<string>("[green]What's your Last Name[/]?"),

            Age = AnsiConsole.Prompt(GetAgeTextPrompt()),

            Hostel = AnsiConsole.Prompt(GetHostelSelectionPrompt())
        };

        AnsiConsole.MarkupLine($"Alright [yellow]{student.FirstName} {student.LastName}[/], welcome!");

        return student;
    }

    /// <summary>
    /// Creates the hostel bar chart.
    /// </summary>
    /// <returns>A BarChart.</returns>
    public static BarChart CreateHostelBarChart()
    {
        var barChart = new BarChart()
        .Width(60)
        .Label("[orange1 bold underline]Number of Students per Hostel[/]")
        .CenterLabel();

        var hostels = StudentsGenerator.Hostels;
        var colors = new List<Color> { Color.Red, Color.Fuchsia, Color.Blue, Color.Yellow, Color.Magenta1 };

        for (var i = 0; i < hostels.Length; i++)
        {
            var hostel = hostels[i];
            var color = colors[i];
            var count = _students.Count(s => s.Hostel == hostel);
            barChart.AddItem(hostel, count, color);
        }

        AnsiConsole.Write(barChart);

        return barChart;
    }

    /// <summary>
    /// Displays the progress.
    /// </summary>
    public static void DisplayProgress()
    {
        var incrementValue = 100 / _students.Count;

        AnsiConsoleRx.Progress()
            .Subscribe(ctx =>
            {
                var streamingTask = ctx.AddTask("Student Streaming");

                foreach (var x in StudentsGenerator.StreamStudentsFromDatabase())
                {
                    streamingTask.Increment(incrementValue);
                }
            });
    }

    /// <summary>
    /// Displays the figlet.
    /// </summary>
    /// <returns>A FigletText.</returns>
    public static FigletText DisplayFiglet()
    {
        var text = new FigletText("Beautify")
            .LeftJustified()
            .Color(Color.Orchid);

        AnsiConsole.Write(text);

        return text;
    }
}
