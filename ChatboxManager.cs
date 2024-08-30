using BuildSoft.VRChat.Osc.Chatbox;
using WebSocketSharp;
using Zuxi.OSC.HeartRate;
using Zuxi.OSC.Modules;
using Zuxi.OSC.utility;

namespace Zuxi.OSC
{
    internal class ChatboxManager
    {
        public static readonly List<string> ChatboxQue = new();
        public static void SendToChatBox(string chatboxText, bool bypass = false)
        {
            if (string.IsNullOrEmpty(chatboxText) && !bypass)
            {
                // Bail Early no update saves VRChat Chat box from throwing a spam error
                return;
            }

            Console.WriteLine(chatboxText);
            OscChatbox.SendMessage(chatboxText, direct: true);
        }

      
        public static void UpdateChatboxFunc()
        {
            // NOTE TO SELF THIS WILL CAUSE A RACE CONTIDION PLEASE FIX (when something calles add new messages to chatboxque while its proccesing)
            if (ChatboxQue.Count > 0)
            {
                SendToChatBox(ChatboxQue[0]);
                // OBSHook64.dll (Real)
              //  File.WriteAllText(Path.Combine(FileUtils.GetAppFolder(), "OBSOUT.txt"), ChatboxQue[0]);
                ChatboxQue.RemoveAt(0);

                return;
            }

            string ChatboxText = "";

            if (Program.NormalChatbox)
            {
                // Disabled lol let me not flex my site my bio does that enough
                // ChatboxText += "imzuxi.com\v";
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
                string currentSong = MediaPlayback.GetCurrentSong();
                if (!string.IsNullOrEmpty(currentSong))
                {
                    ChatboxText += $"[ Current Song ] \v {currentSong}";
                }

                if (!IsInVR)
                {
                    var ProgramWindow = ActiveWindow.Get();
                    
                    if (!string.IsNullOrEmpty(ProgramWindow) && !currentSong.Contains(ProgramWindow) && Console.Title != ProgramWindow)
                    {
                        if (!string.IsNullOrEmpty(currentSong))
                        {
                            ChatboxText += "\v";
                        }
                        ChatboxText += $"[ Current Window ]: \v {ProgramWindow}";
                    }
                }
                #region Broken Will Fix
                //TODO: dont fix jk please fix should be in above if loop
                /*  

                   // SYSINFO


                   if (!string.IsNullOrEmpty(ProgramWindow))
                   {
                       ChatboxText += "\v";
                   }
                 */
                // ChatboxText += "[ System Info ]\v ";
                //   float cpuUsage = SysInfo.GetCpuUsage();
                // SysInfo.GetMemoryUsage(out ulong totalMemory, out ulong usedMemory, out float memoryUsage);
                // ChatboxText += $"CPU: {100 - cpuUsage:F0}% M: {usedMemory / (1024.0 * 1024.0 * 1024.0):F1} / {totalMemory / (1024.0 * 1024.0 * 1024.0):F1} GB";

                // Get Memory Usage 
                //Console.WriteLine($"Memory Usage: ");
                #endregion
            }

            string FileText = System.IO.File.ReadAllText(System.IO.Path.Combine(FileUtils.GetAppFolder(), "chatbox.txt"));
            if (!string.IsNullOrEmpty(FileText))
                ChatboxText += "\v\v" + FileText.Replace("{{env.newline}}", "\v");

            #region Legacy Code

            //  ChatboxText += "\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v\v";

            #endregion
            SendToChatBox(ChatboxText);
        }

        public static void AddNewMessageToChatboxQue(string data)
        {
            ChatboxQue.Add(data);
        }

        #region Timer Loop to Update Chatbox
        static System.Timers.Timer timer;
        internal static bool IsInVR;

        internal static void Start()
        {
            if (timer is not null)
                return;

            timer = new System.Timers.Timer();

            timer.Interval = 7000; // 7 seconds seems to be a safe value to update at

            timer.Elapsed += (s, e) => UpdateChatboxFunc();

            timer.Start();
        }

        #endregion

    }
}
