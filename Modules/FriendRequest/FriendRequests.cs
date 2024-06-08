using Newtonsoft.Json;
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
        public static void FetchVRChatRequestsAndAcceptAll()
        {
            Console.WriteLine("Fetching Friend Requests");

            string Notis = FriendsMain.HClient.GetUserNotis();
            List<JFriendRequest> friendRequests = JFriendRequest.DecodeJson(Notis);

            foreach (JFriendRequest friendRequest in friendRequests)
                AcceptRequest(friendRequest);
            ZuxiBioUpdate.SendUpdate();
        }

        public static void OnWebsocketRequest(string Data)
        {
            WSNotification Noti = WSNotification.FromJson(Data);
            if (Noti.Type == "notification")
            {

                if (Noti.Content.Contains("friendRequest"))
                {
                    JFriendRequest a = JFriendRequest.DecodeJson("[" + Noti.Content + "]")[0];
                    AcceptRequest(a);
                }
            }

            if (Noti.Type == "friend-add")
            {
               WebsocketFriend WF = WebsocketFriend.Create(Noti.Content);
                if (VRCUser.CurrentUser.Friends.Contains(WF.id))
                    return;

                Console.WriteLine("Display Name: " + WF.user.displayName);
                VRCUser.CurrentUser.Friends.Add(WF.id);
                Console.WriteLine(WF.id);

                Console.Title = string.Format("Current User {0} | Friend Count {1}", VRCUser.CurrentUser.DisplayName, VRCUser.CurrentUser.Friends.Count);

                // TODO: fix this to acutally interupt the main function
                ChatBox.SendThisValue.Add(string.Format("Hello {0}\v Thanks for Becoming my Friend!\v I now have {1} Friends!", WF.user.displayName, VRCUser.CurrentUser.Friends.Count));
                ChatBox.UpdateChatbox = false;
                ZuxiBioUpdate.SendUpdate();
            }
        }


        internal static void AcceptRequest(JFriendRequest item)
        {

            if (Config.IgnoredFriendRequests.Contains(item.SenderUserId))
            {
                Console.WriteLine("ignoreing Friend Request From " + item.SenderUserId);
                return;
            }

            string CurrentUser = FriendsMain.HClient.GetVRCUserByID(item.SenderUserId);
            VRCPlayer ThisUser = VRCPlayer.CreateVRCPlayer(CurrentUser);

            if (VRCUser.CurrentUser.Friends.Contains(ThisUser.Id))
                return;

            // Check if the account is more than 30 days old
            TimeSpan accountAge = DateTime.UtcNow - ThisUser.DateJoined;


            if (ThisUser.Tags.Contains("system_trust_basic") || accountAge.TotalDays > 30)
            {

                if (FriendsMain.HClient.AcceptRequest(item.Id))
                {

                    VRCUser.CurrentUser.Friends.Add(item.SenderUserId);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Auto Accepted FriendRequest From {0} id {1} NotiID {2}", item.SenderUsername, item.SenderUserId, item.Id);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Title = string.Format("Current User {0} | Friend Count {1}", VRCUser.CurrentUser.DisplayName, VRCUser.CurrentUser.Friends.Count);

                    FriendsMain.OnNewRequest.Invoke(string.Format("Hello {0}\v Thanks for Becoming my Friend!\v I now have {1} Friends!", item.SenderUsername, VRCUser.CurrentUser.Friends.Count));
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Config.AddUserToIgnored(ThisUser.Id);
                Console.WriteLine("Skipping Accepting Friend Request from user {0} Because there account is still a visiter", item.SenderUsername);

                Console.ForegroundColor = ConsoleColor.Cyan;
            }

        }

    }
}
