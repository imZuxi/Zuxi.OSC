// /*
//  *
//  * Zuxi.OSC - GeneralUtils.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using System.Diagnostics;

namespace Zuxi.OSC.utility;

internal class GeneralUtils
{
    //this is a simple check if steamvr is running to disable outher running stuff in cheatbox manager 
    internal static bool IsInVR()
    {
        Process[] process = Process.GetProcessesByName("vrserver");
        return process.Length > 0;
    }
}