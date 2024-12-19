// /*
//  *
//  * Zuxi.OSC - Main.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Zuxi.OSC.Modules.FriendRequest.Json;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Modules.FriendRequests;

public class FriendsMain
{
    private static WebsocketWrapper? _websocket;

    public static bool Initialize()
    {
       
        Console.WriteLine("Ignored FriendRequest Count: " + Config.GetInstance().IgnoredFriendRequests.Count);
        string authResponse = VRChatAPIClient.GetInstance().CheckAuthStatus();
        if (authResponse.Contains("Missing Credentials") || authResponse.Contains("Requires Two-Factor Authentication"))
        {
           if (!VRChatAPIClient.VRChatAuthenticationFlow.DoAuthFlow(VRChatAPIClient.GetInstance()))
                throw new InvalidOperationException("Failed to Authenticate with VRChat... ");
        }
         

        _websocket = new WebsocketWrapper("wss://pipeline.vrchat.cloud/?authToken=" + Config.GetInstance().AuthCookie,
            FriendRequestHandler.OnWebsocketRequest);
        _websocket.Reconnect();

        VRChatAPIClient.GetInstance().GetLocalUser();

        Console.Title = string.Format("Current User {0} | Friend Count {1}", VRCUser.CurrentUser.DisplayName,
             VRCUser.CurrentUser.Friends.Count);

        Console.WriteLine("Current Friends: {0}", VRCUser.CurrentUser.Friends.Count);

        ChatboxManager.AddNewMessageToChatboxQue(string.Format("Current Friends: {0}",
            VRCUser.CurrentUser.Friends.Count));

        FriendRequestHandler.FetchVrChatRequestsAndAcceptAll();

        return true;
    }
}
