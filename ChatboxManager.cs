// /*
//  *
//  * Zuxi.OSC - ChatboxManager.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using BuildSoft.VRChat.Osc.Chatbox;
using Zuxi.OSC.HeartRate;
using Zuxi.OSC.Modules;
using Zuxi.OSC.utility;

namespace Zuxi.OSC;

internal class ChatboxManager
{
    internal static readonly List<string> ChatboxQue = new();
    private static string _lastSong = "";

    public static void SendToChatBox(string chatboxText, bool bypass = false)
    {
        if (string.IsNullOrEmpty(chatboxText) && !bypass)
        {
            // Bail Early no update saves VRChat Chat box from throwing a spam error
            return;
            chatboxText = "";
        }

        // This is to Debug the Chatbox however its not needed in release but since i un in debug mode more often then not id rather comment this out

        // Console.WriteLine(chatboxText);
        OscChatbox.SendMessage(chatboxText.Trim(), true);
    }


    public static void UpdateChatboxFunc()
    {
        // NOTE TO SELF THIS WILL CAUSE A RACE CONTIDION PLEASE FIX
        // (when something calles add new messages to chatboxque while its proccesing)
        if (ChatboxQue.Count > 0)
        {
            SendToChatBox(ChatboxQue[0]);
            // OBSHook64.dll (Real)
            // File.WriteAllText(Path.Combine(FileUtils.GetAppFolder(), "OBSOUT.txt"), ChatboxQue[0]);
            ChatboxQue.RemoveAt(0);

            return;
        }

        var ChatboxText = "";

        if (Program.NormalChatbox)
        {
            // Disabled lol let me not flex my site my bio does that enough
            // ChatboxText += "imzuxi.com\v";
        }

        if (Program.HeartRate)
        {
            if (HeartBeat.Lasthr != 0)
                ChatboxText += $"{HeartBeat.Lasthr}❤️\v";
        }
        else
        {
            if (!Program.NormalChatbox) ChatboxText += "imzuxi.com";
        }

        if (Program.NormalChatbox)
        {
            var currentSong = MediaPlayback.GetCurrentSong();
            if (!string.IsNullOrEmpty(currentSong) && _lastSong != currentSong)
            {
                if (IsInVR)
                    _lastSong = currentSong;
                ChatboxText += $"[ Current Song ] \v {currentSong}";

               if (!IsInVR)
                    ChatboxText += $"\v{MediaPlayback.getProgressVisual()}";
            }

            if (!IsInVR)
            {
                var ProgramWindow = ActiveWindow.Get();

                if (!string.IsNullOrEmpty(ProgramWindow) && !currentSong.Contains(ProgramWindow) &&
                    Console.Title != ProgramWindow && ProgramWindow.Contains(currentSong.Split('-').FirstOrDefault()))
                {
                    if (!string.IsNullOrEmpty(currentSong)) ChatboxText += "\v";
                    ChatboxText += $"[ Current Window ]: \v {ProgramWindow}";
                }
            }

            #region Broken Will NOT Fix

            //TODO: dont fix jk please fix should be in above if loop edit: BROKEN CANNOT BE ASKED TO FIX IT feel
            //free to fix it

            //  ChatboxText += "\v";
            //  ChatboxText += "[ System Info ]\v ";
            //  float cpuUsage = SystemInfo.GetCpuUsage();
            //  SystemInfo.GetMemoryUsage(out ulong totalMemory, out ulong usedMemory, out float memoryUsage);
            //  ChatboxText += $"CPU: {100 - cpuUsage:F0}% M: {usedMemory / (1024.0 * 1024.0 * 1024.0):F1} / {totalMemory / (1024.0 * 1024.0 * 1024.0):F1} GB";

            //  Get Memory Usage
            //  Console.WriteLine($"Memory Usage: ");

            #endregion
        }

        var FileText = File.ReadAllText(Path.Combine(FileUtils.GetAppFolder(), "chatbox.txt"));
        if (!string.IsNullOrEmpty(FileText))
            ChatboxText += "\v\v" + FileText.Replace("{{env.newline}}", "\v");
        SendToChatBox(ChatboxText);
    }

    public static void AddNewMessageToChatboxQue(string data)
    {
        ChatboxQue.Add(data);
    }

    #region Timer Loop to Update Chatbox

    private static System.Timers.Timer timer;
    internal static bool IsInVR;

    internal static void Start()
    {
        if (timer is not null)
            return;

        timer = new System.Timers.Timer();

        if (IsInVR)
            timer.Interval = 5000; // 3 seconds seems to be a safe value to update at
        else
            timer.Interval = 3000;

        timer.Elapsed += (s, e) => UpdateChatboxFunc();

        timer.Start();
    }

    #endregion
}
