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
    private bool _hasConnected;
    internal bool Shutdown;
    private readonly WebSocket _wss;
    private string ConnectionURL;
    private bool _attemptingConnect;
    private readonly Action<string> _onMessageReceved;
    private int _reconnectionAttempts = 0;
    public WebsocketWrapper(string URI, Action<string> onMessageReceved, Action? onConnected = null)
    {
        _onMessageReceved = onMessageReceved;
        _wss = new WebSocket(URI);

        ConnectionURL = URI.Split('?').First();

        _wss.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        _wss.OnClose += (sender, e) =>
        {
            Console.WriteLine($"{ConnectionURL} Has Closed the Connection attempting reconnect!");
            if (Shutdown) Console.WriteLine("ShutDown Connection");
            if (!_wss.IsAlive)
            {
                Console.WriteLine($"Websocket {ConnectionURL} Reconnecting... ");
                Reconnect();
            }
        };
        _wss.OnOpen += (sender, e) =>
        {
            _attemptingConnect = false;
            if (!_hasConnected)
                Console.WriteLine($"Connected to {ConnectionURL}");

            _hasConnected = true;

            if (onConnected is not null)
                onConnected.Invoke();
        };
        _wss.OnMessage += Ws_OnMessage;
        _wss.Log.Output = (_, __) => { };

      
    }

    internal void Connect()
    {
        Console.WriteLine("Connecting to: " + ConnectionURL);
        _wss.Connect();
    }

    internal WebsocketWrapper GetInstance()
    {
        return this;
    }

    internal void Send(object data)
    {
        _wss.Send(JsonConvert.SerializeObject(data));
    }



    protected internal void Reconnect()
    {
        try
        {
            if (_attemptingConnect)
                return;
            _attemptingConnect = true;


            _reconnectionAttempts++;
            if (_reconnectionAttempts >= 10) throw new Exception($"Failed To Connect to  {_wss.Url}");

            if (_hasConnected)
                Task.Delay(50000).Wait();
            if (!_wss.IsAlive)
                _wss.Connect();
        }
        catch (Exception error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WOAH EXCEPTION THROWN WHILE TRYING TO RECONNECT => " + error);
            Console.ForegroundColor = ConsoleColor.Cyan;
            _wss.Connect();
        }
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        _onMessageReceved.Invoke(e.Data.ToString());
    }
}
