using System.Text.RegularExpressions;

// ReSharper disable ConvertIfStatementToSwitchStatement

namespace Webamoki.Utils;

public static partial class ValueValidations
{
    public static bool HasBannedCharacters(string value) => BannedCharactersRegex().IsMatch(value);

    public static bool CheckGlobalCSS(
        object? value,
        HashSet<object>? validValues = null,
        bool stringPercentage = false,
        bool stringRem = false,
        bool stringPixel = false,
        bool stringInt = false,
        bool stringDouble = false,
        bool allowDouble = false,
        bool allowInt = false,
        bool allowNull = false,
        bool allowNegative = false
    )
    {
        HashSet<object> values = ["initial", "inherit", "unset", "revert", "revert-layer"];
        if (validValues != null) values.UnionWith(validValues);
        return Check(value, values, stringPercentage, stringRem, stringPixel, stringInt, stringDouble, allowDouble,
            allowInt,
            allowNull, allowNegative);
    }

    public static bool Check(
        object? value,
        HashSet<object>? validValues = null,
        bool stringPercentage = false,
        bool stringRem = false,
        bool stringPixel = false,
        bool stringInt = false,
        bool stringDouble = false,
        bool allowDouble = false,
        bool allowInt = false,
        bool allowNull = false,
        bool allowNegative = false
    )
    {
        if (value == null)
        {
            if (allowNull) return true;
            throw new Exception("Null value not allowed");
        }

        if (validValues != null)
        {
            if (validValues.Contains(value)) return true;
        }

        if (value is int intValue)
        {
            if (!allowInt && !allowDouble) throw new Exception("Int value not allowed");
            if (allowNegative) return true;
            if (intValue >= 0) return true;
            throw new Exception("Negative value not allowed");
        }

        if (value is double doubleValue)
        {
            if (!allowDouble) throw new Exception("Double value not allowed");
            if (allowNegative) return true;
            if (doubleValue >= 0) return true;
            throw new Exception("Negative value not allowed");
        }

        // ReSharper disable once InvertIf
        if (
            (stringPixel || stringRem || stringPercentage || stringInt || stringDouble) &&
            value is string str)
        {
            if (allowNegative)
            {
                str = str.TrimStart('-');
            }

            if (stringPercentage)
            {
                str = str.TrimEnd('%');
                stringDouble = true;
            }

            if (stringRem)
            {
                str = str.TrimEnd('m').TrimEnd('e').TrimEnd('r');
                stringDouble = true;
            }

            if (stringPixel && StringPixelRegex().IsMatch(str)) return true;
            if (stringInt && StringIntRegex().IsMatch(str)) return true;
            if ((stringInt || stringDouble) && StringDoubleRegex().IsMatch(str)) return true;
            throw new Exception("Invalid string value");
        }

        throw new Exception("Invalid type");
    }

    [GeneratedRegex(@"^([1-9]\d*|([1-9]\d*\.\d*[1-9])|\.\d*[1-9])$")]
    private static partial Regex StringDoubleRegex();

    [GeneratedRegex(@"^(([1-9]\d*\.\d*[1-9])|\.\d*[1-9])px$")]
    private static partial Regex StringPixelRegex();

    [GeneratedRegex(@"^([1-9]\d*|\d)$")]
    private static partial Regex StringIntRegex();

    [GeneratedRegex(@"/[\<\>\{\}\`\\\\]|<br>|<script|<p|<body|<input|<object|<div|<table|<link|<style|<svg|iframe=|javascript=|onmouseover=|onclick=|onload=|onerror=|background=|size=|rel=|dynsrc=|lowsrc=|src=|href=|<import|http:|https:/i")]
    private static partial Regex BannedCharactersRegex();
}
