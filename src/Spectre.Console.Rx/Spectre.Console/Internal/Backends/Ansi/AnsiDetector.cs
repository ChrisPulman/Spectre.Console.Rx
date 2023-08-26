// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/////////////////////////////////////////////////////////////////////////////////////////////////////
// Portions of this code was ported from the supports-ansi project by Qingrong Ke
// https://github.com/keqingrong/supports-ansi/blob/master/index.js
/////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Spectre.Console.Rx;

internal static class AnsiDetector
{
    private static readonly Regex[] _regexes =
    {
        new("^xterm"), // xterm, PuTTY, Mintty
        new("^rxvt"), // RXVT
        new("^eterm"), // Eterm
        new("^screen"), // GNU screen, tmux
        new("tmux"), // tmux
        new("^vt100"), // DEC VT series
        new("^vt102"), // DEC VT series
        new("^vt220"), // DEC VT series
        new("^vt320"), // DEC VT series
        new("ansi"), // ANSI
        new("scoansi"), // SCO ANSI
        new("cygwin"), // Cygwin, MinGW
        new("linux"), // Linux console
        new("konsole"), // Konsole
        new("bvterm"), // Bitvise SSH Client
        new("^st-256color"), // Suckless Simple Terminal, st
        new("alacritty"), // Alacritty
    };

    public static (bool SupportsAnsi, bool LegacyConsole) Detect(bool stdError, bool upgrade)
    {
        // Running on Windows?
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Running under ConEmu?
            var conEmu = Environment.GetEnvironmentVariable("ConEmuANSI");
            if (!string.IsNullOrEmpty(conEmu) && conEmu.Equals("On", StringComparison.OrdinalIgnoreCase))
            {
                return (true, false);
            }

            var supportsAnsi = Windows.SupportsAnsi(upgrade, stdError, out var legacyConsole);
            return (supportsAnsi, legacyConsole);
        }

        return DetectFromTerm();
    }

    private static (bool SupportsAnsi, bool LegacyConsole) DetectFromTerm()
    {
        // Check if the terminal is of type ANSI/VT100/xterm compatible.
        var term = Environment.GetEnvironmentVariable("TERM");
        if (!string.IsNullOrWhiteSpace(term))
        {
            if (_regexes.Any(regex => regex.IsMatch(term)))
            {
                return (true, false);
            }
        }

        return (false, true);
    }

    [SuppressMessage("Design", "CA1060:Move pinvokes to native methods class", Justification = "Intensional")]
    private static class Windows
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Intensional")]
        private const int STD_OUTPUT_HANDLE = -11;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Intensional")]
        private const int STD_ERROR_HANDLE = -12;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Intensional")]
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Intensional")]
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public static bool SupportsAnsi(bool upgrade, bool stdError, out bool isLegacy)
        {
            isLegacy = false;

            try
            {
                var @out = GetStdHandle(stdError ? STD_ERROR_HANDLE : STD_OUTPUT_HANDLE);
                if (!GetConsoleMode(@out, out var mode))
                {
                    // Could not get console mode, try TERM (set in cygwin, WSL-Shell).
                    var (ansiFromTerm, legacyFromTerm) = DetectFromTerm();

                    isLegacy = ansiFromTerm ? legacyFromTerm : isLegacy;
                    return ansiFromTerm;
                }

                if ((mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) == 0)
                {
                    isLegacy = true;

                    if (!upgrade)
                    {
                        return false;
                    }

                    // Try enable ANSI support.
                    mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                    if (!SetConsoleMode(@out, mode))
                    {
                        // Enabling failed.
                        return false;
                    }

                    isLegacy = false;
                }

                return true;
            }
            catch
            {
                // All we know here is that we don't support ANSI.
                return false;
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
    }
}
