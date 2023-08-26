// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal enum ListPromptInputResult
{
    None = 0,
    Refresh = 1,
    Submit = 2,
    Abort = 3,
}