// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a prompt.
/// </summary>
/// <typeparam name="T">The prompt result type.</typeparam>
public interface IPrompt<T>
{
    /// <summary>
    /// Shows the prompt.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <returns>The prompt input result.</returns>
    T Show(IAnsiConsole console);

    /// <summary>
    /// Shows the prompt asynchronously.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The prompt input result.</returns>
    Task<T> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken);
}