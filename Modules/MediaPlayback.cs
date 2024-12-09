// /*
//  *
//  * Zuxi.OSC - MediaPlayback.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Windows.Media.Control;

namespace Zuxi.OSC.Modules;

internal class MediaPlayback
{
    // Credits https://github.com/VolcanicArts/VRCOSC/blob/ad33e06dcfbbc5497f07f2f29761842dcdaa1bdb/VRCOSC.Modules/Counter/CounterModule.cs#L19
    private const string progress_line = "\u2501";
    private const string progress_dot = "\u25CF";
    private const string progress_start = "\u2523";
    private const string progress_end = "\u252B";

    private const int progress_resolution = 10;

    // https://stackoverflow.com/questions/57580053/
    public static string GetCurrentSong()
    {
        // NO IM NOT MAKING THIS WHOLE APP ASYNC ISTG
        return Task.Run(async () =>
        {
            try
            {
                var gsmtcsm = await GetSystemMediaTransportControlsSessionManager();
                var currentsession = gsmtcsm.GetCurrentSession();
                if (currentsession == null) return "";
                var playbackInfo = currentsession.GetPlaybackInfo();
             //   Console.WriteLine(playbackInfo.PlaybackStatus.ToString());
                if (playbackInfo.PlaybackStatus ==
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused) return "";
                var mediaProperties = await GetMediaProperties(gsmtcsm.GetCurrentSession());

                // Apple Music is kinda stupid ngl this is to fix it i know its stupid idc
                var artist = string.IsNullOrEmpty(mediaProperties.Artist)
                    ? GetStringBeforeFirstDash(mediaProperties.AlbumArtist)
                    : mediaProperties.Artist;
                return string.Format("{1} - {0}", mediaProperties.Title, artist);
            }
            catch (Exception e)
            {
                return "";
            }
        }).Result;
    }

    internal static float GetPlaybackProgress()
    {
        return Task.Run(async () =>
        {
            var gsmtcsm = await GetSystemMediaTransportControlsSessionManager();
            var currentsession = gsmtcsm.GetCurrentSession();
            if (currentsession == null) return 0f; // Return 0 if no session is active

            var timelineProperties = currentsession.GetTimelineProperties();
            var position = timelineProperties.Position;
            var endTime = timelineProperties.EndTime;

            if (endTime == TimeSpan.Zero) return 0f; // Return 0 for live/streaming content or unknown duration

            // Calculate progress as a float (0.0 to 1.0)
            var progress = (float)(position.TotalSeconds / endTime.TotalSeconds);
            return progress;
        }).Result;
    }

    private static float LastPlayBackPercentage = 1000000f; // some high number should prevent it not initializing properly
    // Credits https://github.com/VolcanicArts/VRCOSC/blob/ad33e06dcfbbc5497f07f2f29761842dcdaa1bdb/VRCOSC.Modules/Counter/CounterModule.cs#L179
    internal static string getProgressVisual()
    {
        var percentage = GetPlaybackProgress();
        if (LastPlayBackPercentage == percentage) return "";

        LastPlayBackPercentage = percentage;
        var progressPercentage = progress_resolution * percentage;
        var dotPosition = (int)(MathF.Floor(progressPercentage * 10f) / 10f);
        if (dotPosition < 0 ) return "";

        var visual = progress_start;

        for (var i = 0; i < progress_resolution; i++) visual += i == dotPosition ? progress_dot : progress_line;

        visual += progress_end;

        return visual;
    }


    private static async Task<GlobalSystemMediaTransportControlsSessionManager>
        GetSystemMediaTransportControlsSessionManager()
    {
        return await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
    }

    private static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties> GetMediaProperties(
        GlobalSystemMediaTransportControlsSession session)
    {
        return await session.TryGetMediaPropertiesAsync();
    }

    private static string GetStringBeforeFirstDash(string input)
    {
        var dashIndex = input.IndexOf('—');
        if (dashIndex >= 0) return input.Substring(0, dashIndex);
        return input; // If no dash is found, return the whole input string
    }
}
