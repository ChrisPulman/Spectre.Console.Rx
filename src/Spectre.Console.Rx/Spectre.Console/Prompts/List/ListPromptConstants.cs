// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class ListPromptConstants
{
    public const string Arrow = ">";
    public const string Checkbox = "[[ ]]";
    public const string SelectedCheckbox = "[[[blue]X[/]]]";
    public const string GroupSelectedCheckbox = "[[[grey]X[/]]]";
    public const string InstructionsMarkup = "[grey](Press <space> to select, <enter> to accept)[/]";
    public const string MoreChoicesMarkup = "[grey](Move up and down to reveal more choices)[/]";
}
