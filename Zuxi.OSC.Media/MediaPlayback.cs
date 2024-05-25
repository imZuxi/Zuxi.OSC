using System.Diagnostics;

namespace Zuxi.OSC.Media
{
    public class MediaPlayback
    {
        public static string GetSongInfo()
        {
            string processName = "spotify";

            Process[] SpotifyProcesses = Process.GetProcessesByName(processName);
            foreach (Process process in SpotifyProcesses)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    if (process.MainWindowTitle.Contains("Spotify"))
                        return "";
                    else
                        return process.MainWindowTitle;
                }
            }
            return "[ Paused ]";
        }
    }
}
