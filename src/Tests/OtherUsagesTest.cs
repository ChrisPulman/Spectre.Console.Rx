// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests;

/// <summary>
/// OtherUsagesTest.
/// </summary>
public class OtherUsagesTest
{
    /// <summary>
    /// Whens the student data is passed then render students table.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public Task WhenStudentDataIsPassed_ThenRenderStudentsTable()
    {
        var console = new TestConsole();
        console.Write(SpectreConsoleOtherUsages.CreateStudentTable());

        return Verify(console.Output);
    }

    /// <summary>
    /// Whens the prompting student for age then return error if validation fails.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public Task WhenPromptingStudentForAge_ThenReturnErrorIfValidationFails()
    {
        var console = new TestConsole();
        console.Input.PushTextWithEnter("102");
        console.Input.PushTextWithEnter("ABC");
        console.Input.PushTextWithEnter("99");
        console.Input.PushTextWithEnter("22");

        console.Prompt(SpectreConsoleOtherUsages.GetAgeTextPrompt());

        return Verify(console.Output);
    }

    /// <summary>
    /// Whens the prompting student for hostel then display hotel selection prompt.
    /// </summary>
    [Fact]
    public void WhenPromptingStudentForHostel_ThenDisplayHotelSelectionPrompt()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        var prompt = SpectreConsoleOtherUsages.GetHostelSelectionPrompt();
        prompt.Show(console);

        console.Output.ShouldContain("Choose a Hostel");
        console.Output.ShouldContain("Lincoln");
        console.Output.ShouldContain("Louisa");
        console.Output.ShouldContain("Laurent");
        console.Output.ShouldContain("George");
        console.Output.ShouldContain("Kennedy");
    }

    /// <summary>
    /// Whens the students data is passed then display bar chart.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task WhenStudentsDataIsPassed_ThenDisplayBarChart()
    {
        var console = new TestConsole();
        console.Write(SpectreConsoleOtherUsages.CreateHostelBarChart());

        await Verify(console.Output);
    }

    /// <summary>
    /// Whens the display fig let is invoked then display figlet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task WhenDisplayFigLetIsInvoked_ThenDisplayFiglet()
    {
        var console = new TestConsole().Width(100);
        console.Write(SpectreConsoleOtherUsages.DisplayFiglet());

        await Verify(console.Output);
    }
}
