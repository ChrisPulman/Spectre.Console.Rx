// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests;

/// <summary>
/// BasicUsagesTest.
/// </summary>
public class BasicUsagesTest
{
    private static readonly List<Student> _students = StudentsGenerator.GenerateStudents();

    /// <summary>
    /// Whens the format character is escaped then render text with square brackets.
    /// </summary>
    [Fact]
    public void WhenFormatCharacterIsEscaped_ThenRenderTextWithSquareBrackets()
    {
        var console = new TestConsole();
        console.Markup($"[[{_students[3].FirstName}]][blue][[{_students[3].Hostel}]][/]");
        console.Output.ShouldBe("[Sabrina][George]");
    }

    /// <summary>
    /// Whens the json data is passed then render beautified students data json.
    /// </summary>
    /// <returns>A Task.</returns>
    [Fact]
    public Task WhenJsonDataIsPassed_ThenRenderBeautifiedStudentsDataJson()
    {
        var console = new TestConsole().Size(new Size(100, 25));
        console.Write(SpectreConsoleBasicUsages.BeautifyStudentsDataJson());

        return Verify(console.Output);
    }

    /// <summary>
    /// Whens a calendar is created then pretty print calendar.
    /// </summary>
    /// <returns>A Task.</returns>
    [Fact]
    public Task WhenACalendarIsCreated_ThenPrettyPrintCalendar()
    {
        var console = new TestConsole();
        console.Write(SpectreConsoleBasicUsages.PrettyPrintCalendar());

        return Verify(console.Output);
    }
}
