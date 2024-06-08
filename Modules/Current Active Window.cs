using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zuxi.OSC.Modules
{
    internal class Current_Active_Window
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
                    var WindowName = title.ToString();
                    if (BlacklistedWindows.Any(window => WindowName.ToLower().Contains(window.ToLower())))
                    {
                        return null;
                    }

                    if (WindowName == "Cider") return "Apple Music";
                   
                    GetLastIndex(WindowName, '\\', out WindowName);
                    if (WindowName.Contains("Chrome") || WindowName.Contains("FireFox"))
                        GetLastIndex(WindowName, '-', out WindowName);
                    if (WindowName.Contains("Brave"))
                        GetFirstIndex(WindowName, '-', out WindowName);
                    Replace(WindowName, "Lite", "", out WindowName);
                    
                    WindowName.Replace(".exe", "");
                    return WindowName.Trim();
                }
            }
            return null;

        }
        static string[] BlacklistedWindows = { "vrchat", "task switching", "search" };

        private static void GetLastIndex(string input, char replace, out string output)
        {
            output = input.Split(replace).Last().Trim();
        }

        private static void GetFirstIndex(string input, char replace, out string output)
        {
            output = input.Split(replace).First().Trim();
        }
        private static void Replace(string input, string oldvalue, string newvalue, out string output)
        {
            output = input.Replace(oldvalue, newvalue);
        }
    }


















}

