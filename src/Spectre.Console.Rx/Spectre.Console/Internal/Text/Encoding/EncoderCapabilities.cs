namespace Spectre.Console.Rx;

internal sealed class EncoderCapabilities(ColorSystem colors) : IReadOnlyCapabilities
{
    public ColorSystem ColorSystem { get; } = colors;

    public bool Ansi => false;
    public bool Links => false;
    public bool Legacy => false;
    public bool IsTerminal => false;
    public bool Interactive => false;
    public bool Unicode => true;
    public bool AlternateBuffer => false;
}