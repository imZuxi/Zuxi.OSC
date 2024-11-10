// /*
//  *
//  * Zuxi.OSC - WebsocketWrapper.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;
using WebSocketSharp;

namespace Zuxi.OSC.utility;

internal class WebsocketWrapper
{
    internal bool HasConn;
    internal bool Shutdown;
    internal WebSocket wss;
    internal string ConnectionURL;

    internal Action<string> OnMessageReceved;

    public WebsocketWrapper(string URI, Action<string> onMessageReceved, Action? onConnected = null)
    {
        OnMessageReceved = onMessageReceved;
        wss = new WebSocket(URI);

        ConnectionURL = URI.Split('?').First();

        wss.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        wss.OnClose += (sender, e) =>
        {
            if (Shutdown) Console.WriteLine("ShutDown Connection");
            if (!wss.IsAlive)
                Console.WriteLine($"Websocket {ConnectionURL} Reconnecting... ");
            Reconnect();
        };
        wss.OnOpen += (sender, e) =>
        {
            if (!HasConn)
                Console.WriteLine($"Connected to {ConnectionURL}");
            HasConn = true;
            if (onConnected is not null)
                onConnected.Invoke();
        };
        wss.OnMessage += Ws_OnMessage;
        wss.Log.Output = (_, __) => { };
    }

    internal void Connect()
    {
        Console.WriteLine("Connecting to: " + ConnectionURL);
        wss.Connect();
    }

    internal WebsocketWrapper GetInstance()
    {
        return this;
    }

    internal void Send(object data)
    {
        wss.Send(JsonConvert.SerializeObject(data));
    }

    private static int reconnectcount = 0;

    protected internal void Reconnect()
    {
        try
        {
            reconnectcount++;
            if (reconnectcount >= 10) throw new Exception($"Failed To Connect to  {wss.Url}");
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

    internal void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        OnMessageReceved.Invoke(e.Data.ToString());
    }
}