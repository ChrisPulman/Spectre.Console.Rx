// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal sealed class DefaultPromptValue<T>(T value)
{
    public T Value { get; } = value;
}
