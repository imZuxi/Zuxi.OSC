// /*
//  *
//  * Zuxi.OSC - StringExtensions.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using System.Text.RegularExpressions;
using WebSocketSharp;

namespace Zuxi.OSC.utility;

public static class StringExtensions
{
    public static string RemoveNonUtf8Chars(this string input)
    {
        if (input.IsNullOrEmpty())
        {
            return "";
        }

        // Define the pattern to match UTF-8 characters
        var pattern = @"[^\u0000-\u007F]"; // Matches any character outside the ASCII range (non-UTF-8)

        // Replace non-UTF-8 characters with an empty string
        return Regex.Replace(input, pattern, "");
    }

    public static bool IsgNullOrEmpty(this string input)
    {
        return string.IsNullOrEmpty(input);
    }
}
