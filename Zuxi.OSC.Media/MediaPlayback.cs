using System.Diagnostics;

namespace Zuxi.OSC.Media
{
    public class MediaPlayback
    {/*
        private const string progress_line = "\u2501";
        private const string progress_dot = "\u25CF";
        private const string progress_start = "\u2523";
        private const string progress_end = "\u252B";
        private const int progress_resolution = 10;
        private readonly MediaProvider mediaProvider = new WindowsMediaProvider();
        private string getProgressVisual()
        {
            var progressPercentage = progress_resolution * mediaProvider.State.Timeline.PositionPercentage;
            var dotPosition = (int)(Math.Floor(progressPercentage * 10f) / 10f);

            var visual = progress_start;

            for (var i = 0; i < progress_resolution; i++)
            {
                visual += i == dotPosition ? progress_dot : progress_line;
            }

            visual += progress_end;

            return visual;
        }*/

     

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
