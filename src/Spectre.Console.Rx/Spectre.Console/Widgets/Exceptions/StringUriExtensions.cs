// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal static class StringUriExtensions
{
    public static bool TryGetUri(this string path, [NotNullWhen(true)] out Uri? result)
    {
        try
        {
            if (!Uri.TryCreate(path, UriKind.Absolute, out var uri))
            {
                result = null;
                return false;
            }

            if (uri.Scheme == "file")
            {
                // For local files, we need to append
                // the host name. Otherwise the terminal
                // will most probably not allow it.
                var builder = new UriBuilder(uri)
                {
                    Host = Dns.GetHostName(),
                };

                uri = builder.Uri;
            }

            result = uri;
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}