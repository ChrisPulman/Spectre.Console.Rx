namespace Spectre.Console.Rx.Ansi.Tests;

public sealed class AnsiWriterTests
{
    [Test]
    public void Should_Write_Expected_Ansi()
    {
        // Given
        var fixture = new AnsiFixture();

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe(
            "\e]8;id=123;https://spectreconsole.net\e\\\e[1;3m\e[38;5;11mSpectre Console\e[0m\e]8;;\e\\");
    }

    [Test]
    public void Should_Not_Write_Link_If_Not_Supported()
    {
        // Given
        var fixture = new AnsiFixture();
        fixture.Capabilities.Ansi = true;
        fixture.Capabilities.Links = false;

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe("\e[1;3m\e[38;5;11mSpectre Console\e[0m");
    }

    [Test]
    public void Should_Not_Write_Ansi_If_Not_Supported()
    {
        // Given
        var fixture = new AnsiFixture();
        fixture.Capabilities.Ansi = false;

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe("Spectre Console");
    }

    public sealed class CursorLeft
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorLeft(4);

            // Then
            fixture.Output.ShouldBe("\e[4D");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Steps()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorLeft(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorBackward
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorBackward(4);

            // Then
            fixture.Output.ShouldBe("\e[4D");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Steps()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorBackward(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorRight
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorRight(4);

            // Then
            fixture.Output.ShouldBe("\e[4C");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Steps()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorRight(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorForward
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorForward(4);

            // Then
            fixture.Output.ShouldBe("\e[4C");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Steps()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorForward(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorDown
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorDown(4);

            // Then
            fixture.Output.ShouldBe("\e[4B");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Steps()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorDown(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorUp
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorUp(4);

            // Then
            fixture.Output.ShouldBe("\e[4A");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Steps()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorUp(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorPosition
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorPosition(4, 5);

            // Then
            fixture.Output.ShouldBe("\e[4;5H");
        }
    }

    public sealed class CursorHome
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorHome();

            // Then
            fixture.Output.ShouldBe("\e[H");
        }
    }

    public sealed class EraseInDisplay
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.EraseInDisplay(2);

            // Then
            fixture.Output.ShouldBe("\e[2J");
        }
    }

    public sealed class ClearScrollback
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ClearScrollback();

            // Then
            fixture.Output.ShouldBe("\e[3J");
        }
    }

    public sealed class EraseInLine
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.EraseInLine(2);

            // Then
            fixture.Output.ShouldBe("\e[2K");
        }
    }

    public sealed class ShowCursor
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ShowCursor();

            // Then
            fixture.Output.ShouldBe("\e[?25h");
        }
    }

    public sealed class HideCursor
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.HideCursor();

            // Then
            fixture.Output.ShouldBe("\e[?25l");
        }
    }

    public sealed class SaveCursor
    {
        [Test]
        public void Should_Write_Correct_Ansi_For_Staying_On_Page()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.SaveCursor(true);

            // Then
            fixture.Output.ShouldBe("\e[s");
        }

        [Test]
        public void Should_Write_Correct_Ansi_For_Not_Staying_On_Page()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.SaveCursor(false);

            // Then
            fixture.Output.ShouldBe("\e7");
        }
    }

    public sealed class RestoreCursor
    {
        [Test]
        public void Should_Write_Correct_Ansi_For_Staying_On_Page()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.RestoreCursor(true);

            // Then
            fixture.Output.ShouldBe("\e[u");
        }

        [Test]
        public void Should_Write_Correct_Ansi_For_Not_Staying_On_Page()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.RestoreCursor(false);

            // Then
            fixture.Output.ShouldBe("\e8");
        }
    }

    public sealed class CursorHorizontalAbsolute
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorHorizontalAbsolute(4);

            // Then
            fixture.Output.ShouldBe("\e[4G");
        }
    }

    public sealed class EnterAltScreen
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.EnterAltScreen();

            // Then
            fixture.Output.ShouldBe("\e[?1049h");
        }
    }

    public sealed class ExitAltScreen
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ExitAltScreen();

            // Then
            fixture.Output.ShouldBe("\e[?1049l");
        }
    }

    public sealed class CursorBackwardTabulation
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorBackwardTabulation(4);

            // Then
            fixture.Output.ShouldBe("\e[4Z");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Tabs()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorBackwardTabulation(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorHorizontalTabulation
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorHorizontalTabulation(4);

            // Then
            fixture.Output.ShouldBe("\e[4I");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Tabs()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorHorizontalTabulation(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorNextLine
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorNextLine(4);

            // Then
            fixture.Output.ShouldBe("\e[4E");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Lines()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorNextLine(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class CursorPreviousLine
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorPreviousLine(4);

            // Then
            fixture.Output.ShouldBe("\e[4F");
        }

        [Test]
        public void Should_Not_Write_Ansi_For_Zero_Lines()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorPreviousLine(0);

            // Then
            fixture.Output.ShouldBeEmpty();
        }
    }

    public sealed class Index
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.Index();

            // Then
            fixture.Output.ShouldBe("\eD");
        }
    }

    public sealed class ReverseIndex
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ReverseIndex();

            // Then
            fixture.Output.ShouldBe("\eM");
        }
    }

    public sealed class DeleteCharacter
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.DeleteCharacter(4);

            // Then
            fixture.Output.ShouldBe("\e[4P");
        }
    }

    public sealed class SetCursorStyle
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.SetCursorStyle(3);

            // Then
            fixture.Output.ShouldBe("\e[3 q");
        }
    }

    public sealed class DeleteLine
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.DeleteLine(3);

            // Then
            fixture.Output.ShouldBe("\e[3M");
        }
    }

    public sealed class EraseCharacter
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.EraseCharacter(3);

            // Then
            fixture.Output.ShouldBe("\e[3X");
        }
    }

    public sealed class InsertCharacter
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.InsertCharacter(3);

            // Then
            fixture.Output.ShouldBe("\e[3@");
        }
    }

    public sealed class InsertLine
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.InsertLine(3);

            // Then
            fixture.Output.ShouldBe("\e[3L");
        }
    }

    public sealed class ScrollDown
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ScrollDown(3);

            // Then
            fixture.Output.ShouldBe("\e[3T");
        }
    }

    public sealed class ScrollUp
    {
        [Test]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ScrollUp(3);

            // Then
            fixture.Output.ShouldBe("\e[3S");
        }
    }
}
