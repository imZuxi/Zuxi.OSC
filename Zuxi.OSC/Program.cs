using BuildSoft.VRChat.Osc;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using Zuxi.OSC.Config;
using Zuxi.OSC.Media;
using Zuxi.OSC.Modules;
using Zuxi.OSC.Modules.FriendRequests;
using Zuxi.OSC.Utils;

namespace Zuxi.OSC
{
    internal class Program
    {

        public static bool NormalChatbox = true;
        public static bool HeartRate = true;
        static void Main(string[] args)
        {
            try
            {

              Console.WriteLine(Current_Active_Window.Get());
                Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
                Utils.GeneralUtils.IsVR();

                Directory.SetCurrentDirectory(FileUtils.GetAppFolder());

                JsonConfig.LoadData();

                Console.ForegroundColor = ConsoleColor.Cyan;

                string LastWindow = Environment.NewLine;

                OscConnectionSettings.SendPort = 9005;

              //  OscUtility.Initialize();

             //   Zuxi.OSC.WebModule.WebSocket.CreateSocket();
             
                if (Environment.CommandLine.ToLower().Contains("--zreqo"))
                {
                    NormalChatbox = false;

                    Console.WriteLine(MediaPlayback.GetSongInfo());
                }

                if (Environment.CommandLine.Contains("--zhro"))
                {
                    HeartRate = false;
                }
                else
                {
                    HeartRateMod.Initialize();
                }

                Callbacks.OnNewRequest("Hello World! OSC Ready...");

                ChatBox.StartTimingMe(ChatBox.UpdateChatboxFunc);

                FriendsMain.Initialize(Callbacks.OnNewRequest);
               
                while (true) { }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                ChatBox.UpdateChatboxFunc();

                Callbacks.OnNewRequest("⚠️ Error in Chatbox Alert Zuxi! ⚠️");

                while (ChatBox.SendThisValue.Count < 100)
                {
                    ChatBox.SendThisValue.Add("⚠️ Error in Chatbox Alert Zuxi! ⚠️");
                }

                Console.ReadLine();

            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Application closing...");

            ChatBox.SendToChatBox("", true);
            // give chatbox time to close down
            Thread.Sleep(1000);
            // Perform any necessary cleanup or additional logic here

            // Optionally set e.Cancel to true to prevent the application from closing immediately
            // e.Cancel = true;

            // Exit the application
            Environment.Exit(0);
        }
    }
}

