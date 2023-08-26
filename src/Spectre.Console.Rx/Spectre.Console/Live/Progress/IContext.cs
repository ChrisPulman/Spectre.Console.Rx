// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

/// <summary>
/// IContext.
/// </summary>
public interface IContext
{
    /// <summary>
    /// Refreshes the current progress.
    /// </summary>
    void Refresh();
}
