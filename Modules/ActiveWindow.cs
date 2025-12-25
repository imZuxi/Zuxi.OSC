// /*
//  *
//  * Zuxi.OSC - ActiveWindow.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using System.Runtime.InteropServices;
using System.Text;

namespace Zuxi.OSC.Modules
{
    internal class ActiveWindow
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        internal static string Get()
        {
            IntPtr hWnd = GetForegroundWindow();

            if (hWnd != IntPtr.Zero)
            {
                const int nChars = 256;
                StringBuilder title = new StringBuilder(nChars);

                if (GetWindowText(hWnd, title, nChars) > 0)
                {
                    string WindowName = title.ToString();
                    // Console.WriteLine(title);
                    if (WindowName.Contains("Unity")) //AvatarSearch - VRCDefaultWorldScene - Windows, Mac, Linux - Unity 2022.3.22f1* <DX11>
                    { 
                        StringBuilder builder = new StringBuilder(nChars);

                        GetFirstIndex(WindowName, '-', out WindowName);

                        builder.Append(WindowName);

                        builder.Append("- Unity!");
                        return builder.ToString();

                    }
                    if (BlacklistedWindows.Any(window => WindowName.ToLower().Contains(window.ToLower()))) return null;


                    if (WindowName == "Cider") return "Apple Music";

                    GetLastIndex(WindowName, '\\', out WindowName);
                    if (WindowName.Contains("Chrome") || WindowName.Contains("FireFox") || WindowName.Contains("Waterfox"))
                        GetLastIndex(WindowName, '-', out WindowName);
                    if (WindowName.Contains("Brave"))
                        GetFirstIndex(WindowName, '-', out WindowName);
                    Replace(WindowName, "Lite", "", out WindowName);

                    WindowName.Replace(".exe", "");
                    return WindowName.Trim();
                }
            }
            return "";
        }

        private static readonly string[] BlacklistedWindows =
            { "vrchat", "task switching", "search", "BackgroundModeTrayIconClass", "Spotify Free" };

        private static void GetLastIndex(string input, char replace, out string output)
        {
            output = input.Split(replace).Last().Trim();
        }

        private static void GetFirstIndex(string input, char splittingChar, out string output)
        {
            output = input.Split(splittingChar).First().Trim();
        }

        private static void Replace(string input, string oldvalue, string newvalue, out string output)
        {
            output = input.Replace(oldvalue, newvalue);
        }
    }
}
