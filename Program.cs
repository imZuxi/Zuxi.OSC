﻿using BuildSoft.VRChat.Osc;
using Zuxi.OSC.HeartRate;
using Zuxi.OSC.Modules;
using Zuxi.OSC.Modules.FriendRequests;
using Zuxi.OSC.utility;
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
                // Utils.GeneralUtils.IsVR();
                Console.WriteLine(MediaPlayback.GetCurrentSong());
                Directory.SetCurrentDirectory(FileUtils.GetAppFolder());

                Config.LoadData();

                Console.ForegroundColor = ConsoleColor.Cyan;

                string LastWindow = Environment.NewLine;

                OscConnectionSettings.SendPort = 9000;

                if (Environment.CommandLine.ToLower().Contains("--zreqo"))
                {
                    NormalChatbox = false;

                    Console.WriteLine(MediaPlayback.GetCurrentSong());
                }

                if (Environment.CommandLine.Contains("--zhro"))
                {
                    HeartRate = false;
                }
                else
                {
                    HeartBeat.CreateHeartRate();
                }

                ChatboxManager.AddNewMessageToChatboxQue("Hello World! OSC Ready...");

                ChatboxManager.StartTimingMe(ChatboxManager.UpdateChatboxFunc);

                FriendsMain.Initialize();

                while (true) { }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                ChatboxManager.UpdateChatboxFunc();

               ChatboxManager.AddNewMessageToChatboxQue("⚠️ Error in Chatbox Alert Zuxi! ⚠️");

                while (ChatboxManager.SendThisValue.Count < 100)
                {
                    ChatboxManager.AddNewMessageToChatboxQue("⚠️ Error in Chatbox Alert Zuxi! ⚠️");
                }

                Console.ReadLine();
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Application closing...");

            ChatboxManager.SendToChatBox("", true);
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

