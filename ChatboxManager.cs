﻿// /*
//  *
//  * Zuxi.OSC - ChatboxManager.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using System.Collections.Concurrent;
using BuildSoft.VRChat.Osc.Chatbox;
using Zuxi.OSC.HeartRate;
using Zuxi.OSC.Modules;
using Zuxi.OSC.utility;

namespace Zuxi.OSC;

internal class ChatboxManager
{
   // internal static readonly List<string> ChatboxQue = new();

    internal static readonly ConcurrentQueue<string> ChatboxQue = new();
    private static string _lastSong = "";

    public static void SendToChatBox(string chatboxText, bool bypass = false)
    {
        if (string.IsNullOrEmpty(chatboxText) && !bypass)
        {
            // Bail Early no update saves VRChat Chat box from throwing a spam error
            return;
        }
        OscChatbox.SendMessage(chatboxText.Trim(), true);
    }


    public static void UpdateChatboxFunc()
    {
        if (ChatboxQue.TryDequeue(out string? message))
        {
            SendToChatBox(message);
            return;
        }

        var chatboxText = "";

        if (Program.NormalChatbox)
        {
            // Disabled lol let me not flex my site my bio does that enough
            // ChatboxText += "imzuxi.com\v";
        }

        if (Program.HeartRate)
        {
            if (HeartBeat.Lasthr != 0)
                chatboxText += $"{HeartBeat.Lasthr}❤️\v";
        }

        bool isSendingMusic = false;
        if (Program.NormalChatbox)
        {
            var currentSong = MediaPlayback.GetCurrentSong();
            if (!string.IsNullOrEmpty(currentSong) && _lastSong != currentSong)
            {
                if (IsInVR)
                    _lastSong = currentSong;
                chatboxText += $"[ Current Song ] \v {currentSong}";

               if (!IsInVR)
                    chatboxText += $"\v{MediaPlayback.getProgressVisual()}";
               isSendingMusic = true;
            }

            if (!IsInVR)
            {
                var ProgramWindow = ActiveWindow.Get();

                if (!string.IsNullOrEmpty(ProgramWindow) &&
                    (string.IsNullOrEmpty(currentSong) || (!currentSong.Contains(ProgramWindow) &&
                                                           Console.Title != ProgramWindow &&
                                                           !ProgramWindow.Contains(currentSong.Split('-').FirstOrDefault()))))
                {
                    if (!string.IsNullOrEmpty(currentSong))
                        chatboxText += "\v";
                    chatboxText += $"[ Current Window ]: \v {ProgramWindow}";
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
        try
        {
            var FileText = File.ReadAllText(Path.Combine(FileUtils.GetAppFolder(), "chatbox.txt"));
            if (!string.IsNullOrEmpty(FileText))
            {
                if (isSendingMusic)
                    chatboxText += "\v";
                chatboxText += FileText.Replace("{{env.newline}}", "\v");
            }
        }
        catch (FileNotFoundException)
        {
           File.Create(Path.Combine(FileUtils.GetAppFolder(), "chatbox.txt")).Close();
        }
        SendToChatBox(chatboxText);
    }

    public static void AddNewMessageToChatboxQue(string message)
    {
        ChatboxQue.Enqueue(message);
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
