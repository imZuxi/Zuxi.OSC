using BuildSoft.VRChat.Osc.Chatbox;
using System.Timers;
using Zuxi.OSC.HeartRate;
using Zuxi.OSC.Modules;
using Zuxi.OSC.utility;

namespace Zuxi.OSC
{
    internal class ChatboxManager
    {
        public static bool UpdateChatbox = true;
        public static List<string> SendThisValue = new List<string>();

        public static void SendToChatBox(string ChatboxText, bool bypass = false)
        {
            if (string.IsNullOrEmpty(ChatboxText) && !bypass)
            {

                // Bail Early no update saves VRChat Chatbox from thowing a spam error
                return;
            }

            Console.WriteLine(ChatboxText);
            OscChatbox.SendMessage(ChatboxText, direct: true);
        }

        // NOTE TO SELF THIS WILL CAUSE A RACE CONTIDION PLEASE FIX
        public static void UpdateChatboxFunc()
        {
            UpdateChatbox = true;
            if (SendThisValue.Count > 0)
            {
                SendToChatBox(SendThisValue[0]);
                File.WriteAllText(Path.Combine(FileUtils.GetAppFolder(), "OBSOUT.txt"), SendThisValue[0]);
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
                if (HeartBeat.lasthr != 0)
                    ChatboxText += $"{HeartBeat.lasthr}❤️\v";
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
                string CurrentSong = MediaPlayback.GetCurrentSong();
                if (!string.IsNullOrEmpty(CurrentSong))
                {
                    ChatboxText += $"[ Current Song ] \v {CurrentSong}";
                }

              
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

            string FileText = System.IO.File.ReadAllText(System.IO.Path.Combine(FileUtils.GetAppFolder(), "chatbox.txt"));
            if (!string.IsNullOrEmpty(FileText))
                ChatboxText += "\v\v" + FileText.Replace("{{env.newline}}", "\v");

            #region Legacy Code

            //  ChatboxText += "\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v";

            #endregion
            SendToChatBox(ChatboxText);
            GC.Collect();
        }

        public static void AddNewMessageToChatboxQue(string data)
        {
            ChatboxManager.SendThisValue.Add(data);
        }

        #region Timer Loop to Update Chatbox
        static Action OnTimerFinished = delegate { };
        static System.Timers.Timer timer = null;
        internal static void StartTimingMe(Action OnThisTimerFinished)
        {
            if (timer is not null)
                return;

            timer = new System.Timers.Timer();

            OnTimerFinished = OnThisTimerFinished;

            timer.Interval = 7000; // 1 second

            timer.Elapsed += (s, e) => OnTimerFinished.Invoke();

            timer.Start();
        }

        #endregion

    }
}
