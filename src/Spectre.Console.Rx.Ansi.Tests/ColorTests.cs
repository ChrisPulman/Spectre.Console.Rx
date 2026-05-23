namespace Spectre.Console.Rx.Ansi.Tests;

public sealed class ColorTests
{
    public sealed class TheEqualsMethod
    {
        [Test]
        [Arguments("800080")]
        [Arguments("#800080")]
        public void Should_Consider_Color_And_Color_From_Hex_Equal(string color)
        {
            // Given
            var color1 = new Color(128, 0, 128);

            // When
            var color2 = Color.FromHex(color);

            // Then
            color2.ShouldBe(color1);
        }

        [Test]
        [Arguments("800080")]
        [Arguments("#800080")]
        public void Should_Consider_Color_And_Color_Try_From_Hex_Equal(string color)
        {
            // Given
            var color1 = new Color(128, 0, 128);

            // When
            var result = Color.TryFromHex(color, out var color2);

            // Then
            result.ShouldBeTrue();
            color2.ShouldBe(color1);
        }

        [Test]
        [Arguments(null)]
        [Arguments("")]
        [Arguments("#")]
        [Arguments("#80")]
        [Arguments("FOO")]
        public void Should_Not_Parse_Non_Color_From_Hex(string? input)
        {
            // Given, When
            var result = Record.Exception(() => Color.FromHex(input!));

            // Then
            result.ShouldBeAssignableTo<Exception>();
        }

        [Test]
        [Arguments(null)]
        [Arguments("")]
        [Arguments("#")]
        [Arguments("#80")]
        [Arguments("FOO")]
        public void Should_Not_Parse_Non_Color_Try_From_Hex(string? input)
        {
            // Given, When
            var result = Color.TryFromHex(input!, out var color);

            // Then
            result.ShouldBeFalse();
            color.ShouldBe(Color.Default);
        }

        [Test]
        [Arguments("ffffff")]
        [Arguments("#ffffff")]
        [Arguments("fff")]
        [Arguments("#fff")]
        public void Should_Parse_3_Digit_Hex_Colors_From_Hex(string color)
        {
            // Given
            var expected = new Color(255, 255, 255);

            // When
            var result = Color.FromHex(color);

            // Then
            result.ShouldBe(expected);
        }

        [Test]
        public void Should_Not_Consider_Color_And_Non_Color_Equal()
        {
            // Given
            var color1 = new Color(128, 0, 128);

            // When
            var result = color1.Equals("Foo");

            // Then
            result.ShouldBeFalse();
        }

        [Test]
        public void Should_Consider_Same_Colors_Equal_By_Component()
        {
            // Given
            var color1 = new Color(128, 0, 128);
            var color2 = new Color(128, 0, 128);

            // When
            var result = color1.Equals(color2);

            // Then
            result.ShouldBeTrue();
        }

        [Test]
        public void Should_Consider_Same_Known_Colors_Equal()
        {
            // Given
            var color1 = Color.Cyan1;
            var color2 = Color.Cyan1;

            // When
            var result = color1.Equals(color2);

            // Then
            result.ShouldBeTrue();
        }

        [Test]
        public void Should_Consider_Known_Color_And_Color_With_Same_Components_Equal()
        {
            // Given
            var color1 = Color.Cyan1;
            var color2 = new Color(0, 255, 255);

            // When
            var result = color1.Equals(color2);

            // Then
            result.ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Consider_Different_Colors_Equal()
        {
            // Given
            var color1 = new Color(128, 0, 128);
            var color2 = new Color(128, 128, 128);

            // When
            var result = color1.Equals(color2);

            // Then
            result.ShouldBeFalse();
        }

        [Test]
        public void Shourd_Not_Consider_Black_And_Default_Colors_Equal()
        {
            // Given
            var color1 = Color.Default;
            var color2 = Color.Black;

            // When
            var result = color1.Equals(color2);

            // Then
            result.ShouldBeFalse();
        }
    }

    public sealed class TheGetHashCodeMethod
    {
        [Test]
        public void Should_Return_Same_HashCode_For_Same_Colors()
        {
            // Given
            var color1 = new Color(128, 0, 128);
            var color2 = new Color(128, 0, 128);

            // When
            var hash1 = color1.GetHashCode();
            var hash2 = color2.GetHashCode();

            // Then
            hash1.ShouldBe(hash2);
        }

        [Test]
        public void Should_Return_Different_HashCode_For_Different_Colors()
        {
            // Given
            var color1 = new Color(128, 0, 128);
            var color2 = new Color(128, 128, 128);

            // When
            var hash1 = color1.GetHashCode();
            var hash2 = color2.GetHashCode();

            // Then
            hash1.ShouldNotBe(hash2);
        }
    }

    public sealed class ImplicitConversions
    {
        public sealed class Int32ToColor
        {
            [Test]
            public void Should_Return_Expected_Color()
            {
                for (var number = 0; number < 255; number++)
                {
                    // Given, When
                    var result = (Color)number;

                    // Then
                    result.ShouldBe(Color.FromInt32(number));
                }
            }

            [Test]
            public void Should_Throw_If_Integer_Is_Lower_Than_Zero()
            {
                // Given, When
                var result = Record.Exception(() => _ = (Color)(-1));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("Color number must be between 0 and 255");
            }

            [Test]
            public void Should_Throw_If_Integer_Is_Higher_Than_255()
            {
                // Given, When
                var result = Record.Exception(() => _ = (Color)256);

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("Color number must be between 0 and 255");
            }
        }

        public sealed class ConsoleColorToColor
        {
            [Test]
            [Arguments(ConsoleColor.Black, 0)]
            [Arguments(ConsoleColor.DarkRed, 1)]
            [Arguments(ConsoleColor.DarkGreen, 2)]
            [Arguments(ConsoleColor.DarkYellow, 3)]
            [Arguments(ConsoleColor.DarkBlue, 4)]
            [Arguments(ConsoleColor.DarkMagenta, 5)]
            [Arguments(ConsoleColor.DarkCyan, 6)]
            [Arguments(ConsoleColor.Gray, 7)]
            [Arguments(ConsoleColor.DarkGray, 8)]
            [Arguments(ConsoleColor.Red, 9)]
            [Arguments(ConsoleColor.Green, 10)]
            [Arguments(ConsoleColor.Yellow, 11)]
            [Arguments(ConsoleColor.Blue, 12)]
            [Arguments(ConsoleColor.Magenta, 13)]
            [Arguments(ConsoleColor.Cyan, 14)]
            [Arguments(ConsoleColor.White, 15)]
            public void Should_Return_Expected_Color(ConsoleColor color, int expected)
            {
                // Given, When
                var result = (Color)color;

                // Then
                result.ShouldBe(Color.FromInt32(expected));
            }
        }

        public sealed class ColorToConsoleColor
        {
            [Test]
            [Arguments(0, ConsoleColor.Black)]
            [Arguments(1, ConsoleColor.DarkRed)]
            [Arguments(2, ConsoleColor.DarkGreen)]
            [Arguments(3, ConsoleColor.DarkYellow)]
            [Arguments(4, ConsoleColor.DarkBlue)]
            [Arguments(5, ConsoleColor.DarkMagenta)]
            [Arguments(6, ConsoleColor.DarkCyan)]
            [Arguments(7, ConsoleColor.Gray)]
            [Arguments(8, ConsoleColor.DarkGray)]
            [Arguments(9, ConsoleColor.Red)]
            [Arguments(10, ConsoleColor.Green)]
            [Arguments(11, ConsoleColor.Yellow)]
            [Arguments(12, ConsoleColor.Blue)]
            [Arguments(13, ConsoleColor.Magenta)]
            [Arguments(14, ConsoleColor.Cyan)]
            [Arguments(15, ConsoleColor.White)]
            public void Should_Return_Expected_ConsoleColor_For_Known_Color(int color, ConsoleColor expected)
            {
                // Given, When
                var result = (ConsoleColor)Color.FromInt32(color);

                // Then
                result.ShouldBe(expected);
            }
        }
    }

    public sealed class TheToMarkupMethod
    {
        [Test]
        public void Should_Return_Expected_Markup_For_Default_Color()
        {
            // Given, When
            var result = Color.Default.ToMarkup();

            // Then
            result.ShouldBe("default");
        }

        [Test]
        public void Should_Return_Expected_Markup_For_Known_Color()
        {
            // Given, When
            var result = Color.Red.ToMarkup();

            // Then
            result.ShouldBe("red");
        }

        [Test]
        public void Should_Return_Expected_Markup_For_Custom_Color()
        {
            // Given, When
            var result = new Color(255, 1, 12).ToMarkup();

            // Then
            result.ShouldBe("#FF010C");
        }
    }

    public sealed class TheToStringMethod
    {
        [Test]
        public void Should_Return_Color_Name_For_Known_Colors()
        {
            // Given, When
            var name = Color.Fuchsia.ToString();

            // Then
            name.ShouldBe("fuchsia");
        }

        [Test]
        public void Should_Return_Hex_String_For_Unknown_Colors()
        {
            // Given, When
            var name = new Color(128, 0, 128).ToString();

            // Then
            name.ShouldBe("#800080 (RGB=128,0,128)");
        }
    }
}
