// /*
//  *
//  * Zuxi.OSC - Program.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using BuildSoft.VRChat.Osc;
using Zuxi.OSC.HeartRate;
using Zuxi.OSC.Modules;
using Zuxi.OSC.Modules.FriendRequests;
using Zuxi.OSC.utility;

namespace Zuxi.OSC;

internal class Program
{
    public static bool NormalChatbox = true;
    public static bool HeartRate = true;

    private static void Main(string[] args)
    {
        try
        {
            // Register Ctrl + C
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress!);
            // Call some modules early to ensure they get the correct info and arent null durring console loop
            Console.WriteLine(ActiveWindow.Get());
            Console.WriteLine(MediaPlayback.GetCurrentSong());
            Console.WriteLine(MediaPlayback.getProgressVisual());

            // Set the current directory to %appdata%/zuxi/apps/Zuxi.OSC 
            Directory.SetCurrentDirectory(FileUtils.GetAppFolder());
            // Load Config ie AuthToken.
            Config.LoadData();

            Console.ForegroundColor = ConsoleColor.Cyan;
            // Set the VRChat port
            OscConnectionSettings.SendPort = Config.VRChatOSCPort;

            // launch arg checks
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
                // start heartrate provider
                HeartBeat.CreateHeartRate();
            }

            // Check if in VR then start chatbox & friend request Module 
            ChatboxManager.IsInVR = GeneralUtils.IsInVR();
            ChatboxManager.AddNewMessageToChatboxQue("Hello World! OSC Ready...");
            ChatboxManager.Start();
            FriendsMain.Initialize();
            while (true)
            {
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            ChatboxManager.UpdateChatboxFunc();

            ChatboxManager.AddNewMessageToChatboxQue("⚠️ Error in chat box Alert Zuxi! ⚠️");

            while (ChatboxManager.ChatboxQue.Count < 100)
                ChatboxManager.AddNewMessageToChatboxQue("⚠️ Error in Chat box Alert Zuxi! ⚠️");

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