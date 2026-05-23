// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Spectre.Console.Rx;
using UsingSpectreDotConsole;

var smokeTest = args.Contains("--smoke-test", StringComparer.OrdinalIgnoreCase);
var interactive = AnsiConsole.Profile.Capabilities.Interactive;

if (smokeTest || !interactive)
{
    SpectreConsoleOtherUsages.CreateHostelBarChart();
    _ = SpectreConsoleOtherUsages.GetHostelSelectionPrompt();
    _ = SpectreConsoleOtherUsages.GetAgeTextPrompt();
    SpectreConsoleOtherUsages.CreateStudentTable();
    Console.WriteLine("UsingSpectreConsole initialized.");
    return;
}

SpectreConsoleOtherUsages.CreateHostelBarChart();
SpectreConsoleOtherUsages.GetHostelSelectionPrompt();
SpectreConsoleOtherUsages.PromptStudent();
SpectreConsoleOtherUsages.CreateStudentTable();
SpectreConsoleOtherUsages.GetAgeTextPrompt();
SpectreConsoleOtherUsages.WriteReadableException();
