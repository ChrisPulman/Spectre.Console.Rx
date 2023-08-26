// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// Represents an exclusivity mode.
/// </summary>
public interface IExclusivityMode : IDisposable
{
    /// <summary>
    /// Runs the specified function in exclusive mode.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="func">The func to run in exclusive mode.</param>
    /// <returns>The result of the function.</returns>
    T Run<T>(Func<T> func);

    /// <summary>
    /// Runs the specified function in exclusive mode asynchronously.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="func">The func to run in exclusive mode.</param>
    /// <returns>The result of the function.</returns>
    Task<T> RunAsync<T>(Func<Task<T>> func);
}
