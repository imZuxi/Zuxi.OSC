// /*
//  *
//  * Zuxi.OSC - FileUtils.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

namespace Zuxi.OSC.utility;

internal class FileUtils
{
    public static string GetAppFolder()
    {
        var text = Path.Combine(GetMainFolder(), "Apps", "Zuxi.OSC");
        var flag = !Directory.Exists(text);
        if (flag) Directory.CreateDirectory(text);
        return text;
    }

    internal static string GetMainFolder()
    {
        var text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Zuxi");
        var flag = !Directory.Exists(text);
        if (flag) Directory.CreateDirectory(text);
        return text;
    }
}
