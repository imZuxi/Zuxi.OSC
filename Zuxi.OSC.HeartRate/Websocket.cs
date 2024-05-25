using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Zuxi.OSC.HeartRate
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

                    Console.WriteLine("HypeRate Reconnecting ");
                    tryrecconect();
                };
                wss.OnOpen += (sender, e) =>
                {
                    if (!HasConn)
                        Console.WriteLine($"Connected");
                    HasConn = true;
                    HeartBeat.OnWsConnected();
                };
                wss.OnMessage += Ws_OnMessage;
                wss.Log.Output = (_, __) => { };

            }
        }


        internal static void Send(object data)
        {
            wss.Send(JsonConvert.SerializeObject(data));
        }

        internal protected static void tryrecconect()
        {
            try
            {
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
            
        }

        internal protected static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            HeartBeat.OnWsMessageReceived(e.Data.ToString());

        }

    }

}

