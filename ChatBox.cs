using BuildSoft.VRChat.Osc.Chatbox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Zuxi.OSC.Media;
using Zuxi.OSC.Modules;
using Zuxi.OSC.Utils;

namespace Zuxi.OSC
{
    internal class ChatBox
    {
        public static bool UpdateChatbox = true;
        public static List<string> SendThisValue = new List<string>();

        public static void SendToChatBox(string ChatboxText, bool bypass = false)
        {
            if (string.IsNullOrEmpty(ChatboxText) && !bypass)
            {
                File.WriteAllText(Path.Combine(FileUtils.GetAppFolder(), "OBSOUT.txt"), "");
                // Bail Early no update LMAO saves VRChat Chatbox
                return;
            }
            File.WriteAllText(Path.Combine(FileUtils.GetAppFolder(), "OBSOUT.txt"), ChatboxText);

            Console.WriteLine(ChatboxText);
            OscChatbox.SendMessage(ChatboxText, direct: true);
        }


        public static void UpdateChatboxFunc()
        {
            UpdateChatbox = true;
            if (SendThisValue.Count > 0)
            {
                SendToChatBox(SendThisValue[0]);
                File.WriteAllText(Path.Combine(FileUtils.GetAppFolder(), "OBSOUT.txt"), SendThisValue[0]);
              //  Zuxi.OSC.WebModule.WebSocket.SendMessage(SendThisValue[0]);
                SendThisValue.RemoveAt(0);
               
                return;
            }

            string ChatboxText = "";

            if (Program.NormalChatbox)
            {
              ChatboxText += "imzuxi.com\v";
            }

            if (Program.HeartRate)
            {
                if (HeartRateMod.HeartRateInt != 0)
                    ChatboxText += $"{HeartRateMod.HeartRateInt.ToString()}❤️\v";
            }
            else
            {
                if (!Program.NormalChatbox)
                {
                   ChatboxText += "imzuxi.com";
                }
            }

            if (Program.NormalChatbox)
            {
                string CurrentSong = MediaPlayback.GetSongInfo();
                if (!string.IsNullOrEmpty(CurrentSong))
                {
                    ChatboxText += $"[ Current Song ] \v {CurrentSong}";
                }
               
                if (GeneralUtils.IsVR())
                {
                    var ProgramWindow = Current_Active_Window.Get();
                    if (!string.IsNullOrEmpty(ProgramWindow) && !CurrentSong.Contains(ProgramWindow) && Console.Title != ProgramWindow)
                    {
                        if (!string.IsNullOrEmpty(CurrentSong))
                        {
                            ChatboxText += "\v";
                        }
                        ChatboxText += $"[ Current Window ]: {ProgramWindow}";
                    }

                    // SYSINFO


                    if (!string.IsNullOrEmpty(ProgramWindow))
                    {
                        ChatboxText += "\v";
                    }
                   // ChatboxText += "[ System Info ]\v ";
                 //   float cpuUsage = SysInfo.GetCpuUsage();
                   // SysInfo.GetMemoryUsage(out ulong totalMemory, out ulong usedMemory, out float memoryUsage);
                   // ChatboxText += $"CPU: {100 - cpuUsage:F0}% M: {usedMemory / (1024.0 * 1024.0 * 1024.0):F1} / {totalMemory / (1024.0 * 1024.0 * 1024.0):F1} GB";
                  
                    // Get Memory Usage 
                    Console.WriteLine($"Memory Usage: ");







                }

              
            }

            string FileText = System.IO.File.ReadAllText(System.IO.Path.Combine(FileUtils.GetAppFolder(), "chatbox.txt"));
            if (!string.IsNullOrEmpty(FileText))
                ChatboxText += "\v\v" + FileText.Replace("{{env.newline}}", "\v");

            #region Legacy Code

            //  ChatboxText += "\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v";

            #endregion
            SendToChatBox(ChatboxText);
            GC.Collect();
        }

        #region Timer Loop to Update Chatbox
        static Action OnTimerFinished = delegate { };
        static Timer timer = null;
        internal static void StartTimingMe(Action OnThisTimerFinished)
        {
            if (timer is not null)
                return;

            timer = new Timer();

            OnTimerFinished = OnThisTimerFinished;

            timer.Interval = 7000; // 1 second

            timer.Elapsed += (s,e) => OnTimerFinished.Invoke();

            timer.Start();
        }

        #endregion

    }
}
