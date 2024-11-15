﻿// /*
//  *
//  * Zuxi.OSC - PrimitiveExtensions.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

namespace VRCHypeRate.Utils;

public static class PrimitiveExtensions
{
    public static int[] ToDigitArray(this int num, int totalSize)
    {
        var numStr = num.ToString().PadLeft(totalSize, '0');
        var intList = numStr.Select(digit => int.Parse(digit.ToString()));
        return intList.ToArray();
    }
}