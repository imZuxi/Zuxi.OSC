﻿using System.Linq;

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