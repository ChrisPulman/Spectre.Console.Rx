// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal readonly struct ProgressSample(DateTime timestamp, double value)
{
    public double Value { get; } = value;

    public DateTime Timestamp { get; } = timestamp;
}
