using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Zuxi.OSC.Modules.FriendRequests
{

    internal class Websocket
    {
        internal bool HasConn;
        internal bool Shutdown;
        internal protected static WebSocket wss;
        public Websocket(string URI)
        {
            using (wss = new WebSocket(URI))
            {
                Console.WriteLine("Connecting to: " + wss.Url);
                wss.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                Task.Delay(60000);


                wss.Connect();

                wss.OnClose += (sender, e) =>
                {
                    if (Shutdown)
                    { Console.WriteLine("ShutDown Connection"); }

                    Console.WriteLine("VRChat Reconnecting... ");
                    tryrecconect();
                };
                wss.OnOpen += (sender, e) =>
                {
                    if (!HasConn)
                        Console.WriteLine($"VRChat WebsocketPipeline Connected");
                    HasConn = true;
                 //   HeartBeat.OnWsConnected();
                };
                wss.OnMessage += Ws_OnMessage;
                wss.Log.Output = (_, __) => { };

            }
        }


        internal static void Send(object data)
        {
            wss.Send(JsonConvert.SerializeObject(data));
        }
        static int reconnectcount = 0;
        internal protected static void tryrecconect()
        {
            try
            {
                reconnectcount++;
                if (reconnectcount >= 10) { throw new Exception("Failed To Connect to VRChat Check Auth Cookie"); }
                Task.Delay(50000);
                if (!wss.IsAlive)
                    wss.Connect();
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("WOAH EXCEPTION THROWN WHILE TRYING TO RECONNECT => " + error);
                Console.ForegroundColor = ConsoleColor.Cyan;
                wss.Connect();
            }
        }
        internal static void ws_OnConnected()
        {
           Console.WriteLine("VRChat Connected!");
        }

        internal protected static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            FriendRequests.OnWebsocketRequest(e.Data.ToString());

        }

    }

}


