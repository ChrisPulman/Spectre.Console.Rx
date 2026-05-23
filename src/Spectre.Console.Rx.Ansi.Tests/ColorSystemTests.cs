namespace Spectre.Console.Rx.Ansi.Tests;

public sealed class ColorSystemTests
{
    [Test]
    [Arguments(ColorSystem.NoColors, ColorSystemSupport.NoColors)]
    [Arguments(ColorSystem.Legacy, ColorSystemSupport.Legacy)]
    [Arguments(ColorSystem.Standard, ColorSystemSupport.Standard)]
    [Arguments(ColorSystem.EightBit, ColorSystemSupport.EightBit)]
    [Arguments(ColorSystem.TrueColor, ColorSystemSupport.TrueColor)]
    public void Should_Be_Analog_To_ColorSystemSupport(ColorSystem colors, ColorSystemSupport support)
    {
        // Given, When
        var result = (int)colors;

        // Then
        result.ShouldBe((int)support);
    }
}