﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Zuxi.OSC.Module.FriendRequests;
using Zuxi.OSC.Modules.FriendRequest.Json;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Modules.FriendRequests
{
    internal class FriendRequestHandler
    {
        public static void FetchVrChatRequestsAndAcceptAll()
        {
            Console.WriteLine("Fetching Friend Requests");

            string userNotifs = HClient.GetInstance().GetUserNotis();
            List<friendRequest> friendRequests = friendRequest.DecodeJson(userNotifs);

            foreach (friendRequest friendRequest in friendRequests)
                AcceptRequest(friendRequest);
            ZuxiBioUpdate.SendUpdate();
        }

        public static void OnWebsocketRequest(string data)
        {
            WSNotification wsNotification = WSNotification.FromJson(data);
            if (wsNotification.Type == "notification")
            {

                if (wsNotification.Content.Contains("friendRequest"))
                {
                    friendRequest a = friendRequest.DecodeJson("[" + wsNotification.Content + "]")[0];
                    AcceptRequest(a);
                }
            }

            if (wsNotification.Type == "friend-add")
            {
               WebsocketFriend websocketFriend = WebsocketFriend.Create(wsNotification.Content);
                if (VRCUser.CurrentUser.Friends.Contains(websocketFriend.id))
                    return;

                Console.WriteLine("Display Name: " + websocketFriend.user.displayName);
                VRCUser.CurrentUser.Friends.Add(websocketFriend.id);
                Console.WriteLine(websocketFriend.id);

                Console.Title =
                    $"Current User {VRCUser.CurrentUser.DisplayName} | Friend Count {VRCUser.CurrentUser.Friends.Count}";

                 ChatboxManager.AddNewMessageToChatboxQue(
                     $"Hello {websocketFriend.user.displayName}\v Thanks for Becoming my Friend!\v I now have {VRCUser.CurrentUser.Friends.Count} Friends!");
                ZuxiBioUpdate.SendUpdate();
            }

            if (wsNotification.Type == "friend-delete")
            {
                WebsocketFriend wf = WebsocketFriend.Create(wsNotification.Content);
                if (!VRCUser.CurrentUser.Friends.Contains(wf.id))
                    return;
                VRCPlayer vrcUser =  HClient.GetInstance().GetVRCUserByID(wf.id);
                Console.WriteLine($"{vrcUser.DisplayName} Removed you as a friend!");
               
            }
        }


        private static void AcceptRequest(friendRequest item)
        {

            if (Config.IgnoredFriendRequests.Contains(item.SenderUserId))
            {
                Console.WriteLine("ignoring Friend Request From " + item.SenderUserId);
                return;
            }

            VRCPlayer vrcUser  = HClient.GetInstance().GetVRCUserByID(item.SenderUserId);


            if (VRCUser.CurrentUser.Friends.Contains(vrcUser.Id))
                return;

            // Check if the account is more than 30 days old
            TimeSpan accountAge = DateTime.UtcNow - vrcUser.DateJoined;


            if (vrcUser.Tags.Contains("system_trust_basic") || accountAge.TotalDays > 30)
            {
                if (!HClient.GetInstance().AcceptRequest(item.Id)) return;
                VRCUser.CurrentUser.Friends.Add(item.SenderUserId);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Auto Accepted FriendRequest From {0} id {1} NotiID {2}", item.SenderUsername, item.SenderUserId, item.Id);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Title =
                    $"Current User {VRCUser.CurrentUser.DisplayName} | Friend Count {VRCUser.CurrentUser.Friends.Count}";

                ChatboxManager.AddNewMessageToChatboxQue(
                    $"Hello {item.SenderUsername}\v Thanks for Becoming my Friend!\v I now have {VRCUser.CurrentUser.Friends.Count} Friends!");

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Config.AddUserToIgnored(vrcUser.Id);
                Console.WriteLine("Skipping Accepting Friend Request from user {0} Because there account is still a visiter", item.SenderUsername);

                Console.ForegroundColor = ConsoleColor.Cyan;
            }

        }

    }
}
