using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using WebSocketSharp;
using WebSocketSharp.Server;
namespace Zuxi.OSC.WebModule
{
    internal class WBehavior : WebSocketBehavior
    {
     public static List<WBehavior> instances = new List<WBehavior>();

        public WBehavior() {
            Console.WriteLine("New Client Connected.");
            instances.Add(this);
        }


        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "ping")
            {
                Send("pong");
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            instances.Remove(this); 
        }

        public void SendMessage (string Message)
        {
            Send(Message);
        }



    }

    internal class Message
    {
        bool update = false;
        string message = string.Empty;
    }


}
