// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// A prompt that is answered with a yes or no.
/// </summary>
public sealed class ConfirmationPrompt : IPrompt<bool>
{
    private readonly string _prompt;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmationPrompt"/> class.
    /// </summary>
    /// <param name="prompt">The prompt markup text.</param>
    public ConfirmationPrompt(string prompt) => _prompt = prompt ?? throw new System.ArgumentNullException(nameof(prompt));

    /// <summary>
    /// Gets or sets the character that represents "yes".
    /// </summary>
    public char Yes { get; set; } = 'y';

    /// <summary>
    /// Gets or sets the character that represents "no".
    /// </summary>
    public char No { get; set; } = 'n';

    /// <summary>
    /// Gets or sets a value indicating whether "yes" is the default answer.
    /// </summary>
    public bool DefaultValue { get; set; } = true;

    /// <summary>
    /// Gets or sets the message for invalid choices.
    /// </summary>
    public string InvalidChoiceMessage { get; set; } = "[red]Please select one of the available options[/]";

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// choices should be shown.
    /// </summary>
    public bool ShowChoices { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// default values should be shown.
    /// </summary>
    public bool ShowDefaultValue { get; set; } = true;

    /// <summary>
    /// Gets or sets the string comparer to use when comparing user input
    /// against Yes/No choices.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="StringComparer.CurrentCultureIgnoreCase"/>.
    /// </remarks>
    public StringComparer Comparer { get; set; } = StringComparer.CurrentCultureIgnoreCase;

    /// <inheritdoc/>
    public bool Show(IAnsiConsole console) => ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();

    /// <inheritdoc/>
    public async Task<bool> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        var comparer = Comparer ?? StringComparer.CurrentCultureIgnoreCase;

        var prompt = new TextPrompt<char>(_prompt, comparer)
            .InvalidChoiceMessage(InvalidChoiceMessage)
            .ValidationErrorMessage(InvalidChoiceMessage)
            .ShowChoices(ShowChoices)
            .ShowDefaultValue(ShowDefaultValue)
            .DefaultValue(DefaultValue ? Yes : No)
            .AddChoice(Yes)
            .AddChoice(No);

        var result = await prompt.ShowAsync(console, cancellationToken).ConfigureAwait(false);

        return comparer.Compare(Yes.ToString(), result.ToString()) == 0;
    }
}
