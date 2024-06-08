using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuxi.OSC
{
    internal class Callbacks
    {
        public static void OnNewRequest(string Data)
        {
            ChatBox.SendThisValue.Add("imzuxi.com\v" + Data);
            ChatBox.UpdateChatbox = false;
        }

      
    }
}
