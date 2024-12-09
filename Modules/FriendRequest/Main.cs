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
        Console.WriteLine("AuthCookie: " + Config.AuthCookie);

        Console.WriteLine("Ignored FriendRequest Count: " + Config.IgnoredFriendRequests.Count);

         if (VRChatAPIClient.GetInstance().CheckAuthStatus().Contains("Missing Credentials"))
            throw new Exception("Failed Auth Check With VRChat Check Auth Cookie");
        _websocket = new WebsocketWrapper("wss://pipeline.vrchat.cloud/?authToken=" + Config.AuthCookie,
            FriendRequestHandler.OnWebsocketRequest);
        _websocket.Reconnect();
        var userinfojson = VRChatAPIClient.GetInstance().GetLocalUser();

        VRCUser.CreateVRCUser(userinfojson);

        Console.Title = string.Format("Current User {0} | Friend Count {1}", VRCUser.CurrentUser.DisplayName,
            VRCUser.CurrentUser.Friends.Count);

        Console.WriteLine("Current Friends: {0}", VRCUser.CurrentUser.Friends.Count);

        ChatboxManager.AddNewMessageToChatboxQue(string.Format("Current Friends: {0}",
            VRCUser.CurrentUser.Friends.Count));

        FriendRequestHandler.FetchVrChatRequestsAndAcceptAll();

        return true;
    }
}
