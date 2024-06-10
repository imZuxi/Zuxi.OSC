using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Windows.Media.Control;
namespace Zuxi.OSC.Modules
{
    internal class MediaPlayback
    {
        // https://stackoverflow.com/questions/57580053/
        public static string GetCurrentSong()
        {
            // NO IM NOT MAKING THIS WHOLE APP ASYNC ISTG
            return Task.Run(async () =>
            {
                // GOD THIS IS SO UNOPTIMIZXED HOLY but cannot be asked to fix
                // btw this took 3 hours to figure out since this project needed to be upgraded to .net core so i used net 8 org now 6 lmao 

                var gsmtcsm = await GetSystemMediaTransportControlsSessionManager();

                var playbackInfo = gsmtcsm.GetCurrentSession().GetPlaybackInfo();

                Console.WriteLine(playbackInfo.PlaybackStatus.ToString());
                if (playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                {
                    return "";
                }

                var mediaProperties = await GetMediaProperties(gsmtcsm.GetCurrentSession());



                //  if (mediaProperties.)
                // Apple Music is kinda stupid ngl this is to fix it i know its stupid idc
                string artist = string.IsNullOrEmpty(mediaProperties.Artist) ? GetStringBeforeFirstDash(mediaProperties.AlbumArtist) : mediaProperties.Artist;
                 return string.Format("{0} - {1}", mediaProperties.Title, artist);

            }).Result;
        }

        private static async Task<GlobalSystemMediaTransportControlsSessionManager> GetSystemMediaTransportControlsSessionManager() =>
        await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

        private static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties> GetMediaProperties(GlobalSystemMediaTransportControlsSession session) =>
            await session.TryGetMediaPropertiesAsync();

        static string GetStringBeforeFirstDash(string input)
        {
            int dashIndex = input.IndexOf('—');
            if (dashIndex >= 0)
            {
                return input.Substring(0, dashIndex);
            }
            return input;  // If no dash is found, return the whole input string
        }
    }
}
