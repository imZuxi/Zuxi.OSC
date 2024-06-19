using BuildSoft.VRChat.Osc;
using System.Diagnostics;
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
            
                Console.WriteLine(ActiveWindow.Get());
                Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress!);
                Console.WriteLine(MediaPlayback.GetCurrentSong());
                Directory.SetCurrentDirectory(FileUtils.GetAppFolder());
                Config.LoadData();
                Console.ForegroundColor = ConsoleColor.Cyan;
                OscConnectionSettings.SendPort = 9000;

                if (Environment.CommandLine.ToLower().Contains("--zreqo"))
                {
                    Console.WriteLine("Normal chat box Disabled...");
                    NormalChatbox = false;
                }

                if (Environment.CommandLine.ToLower().Contains("--zhro"))
                {
                    Console.WriteLine("Heartrate Disabled...");
                    HeartRate = false;
                }
                else
                {
                    HeartBeat.CreateHeartRate();
                }

                ChatboxManager.IsInVR = GeneralUtils.IsInVR();
                ChatboxManager.AddNewMessageToChatboxQue("Hello World! OSC Ready...");
                ChatboxManager.Start();
                FriendsMain.Initialize();
                while (true) { }
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Error: {ex}");
                ChatboxManager.UpdateChatboxFunc();

                ChatboxManager.AddNewMessageToChatboxQue("⚠️ Error in chat box Alert Zuxi! ⚠️");

                while (ChatboxManager.ChatboxQue.Count < 100)
                {
                    ChatboxManager.AddNewMessageToChatboxQue("⚠️ Error in Chat box Alert Zuxi! ⚠️");
                }

                Console.ReadLine();
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Application closing...");

            ChatboxManager.SendToChatBox("", true);
            Thread.Sleep(1000);
         
            Environment.Exit(0);
        }
    }
}

