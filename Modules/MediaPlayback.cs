using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var mediaProperties = await GetMediaProperties(gsmtcsm.GetCurrentSession());
                // Apple Music is kinda stupid ngl this is to fix it i know its stupid idc
                return string.IsNullOrEmpty(mediaProperties.Artist) ? mediaProperties.AlbumArtist.Split('-')[0] : 
                string.Format("{0} - {1}", mediaProperties.Title, mediaProperties.Artist);

            }).Result;
        }

        private static async Task<GlobalSystemMediaTransportControlsSessionManager> GetSystemMediaTransportControlsSessionManager() =>
        await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

        private static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties> GetMediaProperties(GlobalSystemMediaTransportControlsSession session) =>
            await session.TryGetMediaPropertiesAsync();
    }
}
