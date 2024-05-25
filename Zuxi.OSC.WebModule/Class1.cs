using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Zuxi.OSC.WebModule
{
    public class WebSocket
    {

        public static void SendMessage(string _s)
        {
            try
            {
                foreach (WBehavior a in WBehavior.instances)
                {
                    if (a != null)
                    {
                        a.SendMessage(_s.Replace("\v", ""));
                    }

                }
            }
            catch (Exception e) { 
           // Console.WriteLine(e.ToString());
            }
           
        }
        public static List<string> usrs = new List<string>();
       public static void CreateSocket()
        {
            var wssv = new WebSocketServer(6498);
            wssv.AddWebSocketService<WBehavior>("/");
            wssv.Start();
            // wssv.WebSocketServices
            Console.WriteLine("Websocket Server Init");
        }

        public static void SendMessageToConnectedClients(string message)
        {

        }
        public static void AddUser(string _string)
        {
            usrs.Add( _string );
        }
    }
}
