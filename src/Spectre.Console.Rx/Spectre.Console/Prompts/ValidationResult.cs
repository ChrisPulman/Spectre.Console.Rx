// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents a prompt validation result.
/// </summary>
public sealed class ValidationResult
{
    private ValidationResult(bool successful, string? message)
    {
        Successful = successful;
        Message = message;
    }

    /// <summary>
    /// Gets a value indicating whether or not validation was successful.
    /// </summary>
    public bool Successful { get; }

    /// <summary>
    /// Gets the validation error message.
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// Returns a <see cref="ValidationResult"/> representing successful validation.
    /// </summary>
    /// <returns>The validation result.</returns>
    public static ValidationResult Success() => new(true, null);

    /// <summary>
    /// Returns a <see cref="ValidationResult"/> representing a validation error.
    /// </summary>
    /// <param name="message">The validation error message, or <c>null</c> to show the default validation error message.</param>
    /// <returns>The validation result.</returns>
    public static ValidationResult Error(string? message = null) => new(false, message);
}
