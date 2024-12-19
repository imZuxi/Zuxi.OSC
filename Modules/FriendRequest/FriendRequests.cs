// /*
//  *
//  * Zuxi.OSC - FriendRequests.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Zuxi.OSC.Module.FriendRequests;
using Zuxi.OSC.Modules.FriendRequest.Json;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Modules.FriendRequests;

internal class FriendRequestHandler
{
    public static void FetchVrChatRequestsAndAcceptAll()
    {
        Console.WriteLine("Fetching Friend Requests");

        var userNotifs = VRChatAPIClient.GetInstance().GetUserNotis();
        List<friendRequest> friendRequests = friendRequest.DecodeJson(userNotifs);

        foreach (var friendRequest in friendRequests)
            AcceptRequest(friendRequest);
        ZuxiBioUpdate.SendUpdate();
    }

    public static void OnWebsocketRequest(string data)
    {
        // Todo: mark all api responses here as a basic to keep up with them
        var wsNotification = WSNotification.FromJson(data);

        if (wsNotification.Type == "notification")
            if (wsNotification.Content.Contains("friendRequest"))
            {
                var wsfriendRequest = friendRequest.DecodeJson("[" + wsNotification.Content + "]")[0];
                AcceptRequest(wsfriendRequest);
            }

        if (wsNotification.Type == "friend-add")
        {
            var websocketFriend = WebsocketFriend.Create(wsNotification.Content);
            if (VRCUser.CurrentUser.Friends.Contains(websocketFriend.id))
                return;

            Console.WriteLine("Display Name: {0} Id: {1}", websocketFriend.user.displayName, websocketFriend.id);
            VRCUser.CurrentUser.Friends.Add(websocketFriend.id);

            Console.Title =
                $"Current User {VRCUser.CurrentUser.DisplayName} | Friend Count {VRCUser.CurrentUser.Friends.Count}";

            ChatboxManager.AddNewMessageToChatboxQue(
                $"Hello {websocketFriend.user.displayName}\v Thanks for Becoming my Friend!\v I now have {VRCUser.CurrentUser.Friends.Count} Friends!");
            ZuxiBioUpdate.SendUpdate();
        }

        if (wsNotification.Type == "friend-delete")
        {
            var wf = WebsocketFriend.Create(wsNotification.Content);
            if (!VRCUser.CurrentUser.Friends.Contains(wf.id))
                return;
            var vrcUser = VRChatAPIClient.GetInstance().GetVRCUserByID(wf.id);
            Console.WriteLine("{0} Removed you as a friend!", vrcUser.DisplayName);
        }
    }


    private static void AcceptRequest(friendRequest item)
    {
        if (Config.GetInstance().IgnoredFriendRequests.Contains(item.SenderUserId))
        {
            Console.WriteLine("ignoring Friend Request From " + item.SenderUserId);
            return;
        }

        var vrcUser = VRChatAPIClient.GetInstance().GetVRCUserByID(item.SenderUserId);


        if (VRCUser.CurrentUser.Friends.Contains(vrcUser.Id))
            return;

        // Check if the account is more than 30 days old
        var accountAge = DateTime.UtcNow - vrcUser.DateJoined;


        if (vrcUser.Tags.Contains("system_trust_basic") || accountAge.TotalDays > 30)
        {
            if (!VRChatAPIClient.GetInstance().AcceptRequest(item.Id)) return;
            VRCUser.CurrentUser.Friends.Add(item.SenderUserId);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Auto Accepted FriendRequest From {0} id {1} NotiID {2}", item.SenderUsername,
                item.SenderUserId, item.Id);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Title =
                $"Current User {VRCUser.CurrentUser.DisplayName} | Friend Count {VRCUser.CurrentUser.Friends.Count}";

            ChatboxManager.AddNewMessageToChatboxQue(
                $"Hello {item.SenderUsername}\v Thanks for Becoming my Friend!\v I now have {VRCUser.CurrentUser.Friends.Count} Friends!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Config.GetInstance().AddUserToIgnored(vrcUser.Id);
            Console.WriteLine(
                "Skipping Accepting Friend Request from user {0} Because there account is still a visitor",
                item.SenderUsername);

            Console.ForegroundColor = ConsoleColor.Cyan;
        }
    }
}
