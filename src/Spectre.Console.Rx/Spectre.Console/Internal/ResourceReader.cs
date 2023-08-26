// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class ResourceReader
{
    public static string ReadManifestData(string resourceName)
    {
        if (resourceName is null)
        {
            throw new ArgumentNullException(nameof(resourceName));
        }

        var assembly = typeof(ResourceReader).Assembly;
        resourceName = resourceName.ReplaceExact("/", ".");

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd().NormalizeNewLines();
            }
        }
    }
}