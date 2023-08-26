// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx;

internal struct FileSize
{
    public FileSize(double bytes)
    {
        Bytes = bytes;
        Unit = Detect(bytes);
    }

    public FileSize(double bytes, FileSizeUnit unit)
    {
        Bytes = bytes;
        Unit = unit;
    }

    public double Bytes { get; }

    public FileSizeUnit Unit { get; }

    public string Suffix => GetSuffix();

    public string Format(CultureInfo? culture = null)
    {
        var @base = GetBase(Unit);
        if (@base == 0)
        {
            @base = 1;
        }

        var bytes = Bytes / @base;

        return Unit == FileSizeUnit.Byte
            ? ((int)bytes).ToString(culture ?? CultureInfo.InvariantCulture)
            : bytes.ToString("F1", culture ?? CultureInfo.InvariantCulture);
    }

    public override string ToString() => ToString(suffix: true, CultureInfo.InvariantCulture);

    public string ToString(bool suffix = true, CultureInfo? culture = null)
    {
        if (suffix)
        {
            return $"{Format(culture)} {Suffix}";
        }

        return Format(culture);
    }

    private static FileSizeUnit Detect(double bytes)
    {
        foreach (var unit in (FileSizeUnit[])Enum.GetValues(typeof(FileSizeUnit)))
        {
            if (bytes < (GetBase(unit) * 1024))
            {
                return unit;
            }
        }

        return FileSizeUnit.Byte;
    }

    private static double GetBase(FileSizeUnit unit) => Math.Pow(1024, (int)unit);

    private string GetSuffix() => (Bytes, Unit) switch
    {
        (_, FileSizeUnit.KiloByte) => "KB",
        (_, FileSizeUnit.MegaByte) => "MB",
        (_, FileSizeUnit.GigaByte) => "GB",
        (_, FileSizeUnit.TeraByte) => "TB",
        (_, FileSizeUnit.PetaByte) => "PB",
        (_, FileSizeUnit.ExaByte) => "EB",
        (_, FileSizeUnit.ZettaByte) => "ZB",
        (_, FileSizeUnit.YottaByte) => "YB",
        (1, _) => "byte",
        (_, _) => "bytes",
    };
}
