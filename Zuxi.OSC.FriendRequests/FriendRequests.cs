using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Zuxi.OSC.Config;
using Zuxi.OSC.Module.FriendRequests;
using Zuxi.OSC.Module.FriendRequests.JSONUtils;

namespace Zuxi.OSC.Modules.FriendRequests
{
    internal class FriendRequests
    {
        public static void FetchVRChatRequestsAndAcceptAll()
        {
           // TimerUtils.StopTimer();


            ///     if (Process.GetProcessesByName("VRChat").FirstOrDefault() is null)
            //  {
            // TimerUtils.StartTimingMe(FetchVRChatRequestsAndAcceptAll);
            //    return;
            // }

            Console.WriteLine("Fetching Friend Requests");

            string Notis = FriendsMain.HClient.GetUserNotis();
            List<FriendRequest> friendRequests = DecodeJson(Notis);

            foreach (FriendRequest friendRequest in friendRequests)
                AcceptRequest(friendRequest);

            ZuxiBioUpdate.SendUpdate();
          

            //   TimerUtils.StartTimingMe(FetchVRChatRequestsAndAcceptAll);

        }

        public static void OnWebsocketRequest(string Data)
        {
            TNotification Noti = TNotification.FromJson(Data);
            if (Noti.Type == "notification")
            {

                if (Noti.Content.Contains("friendRequest"))
                {
                    FriendRequest a = DecodeJson("[" + Noti.Content + "]")[0];
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

                FriendsMain.OnNewRequest.Invoke(string.Format("Hello {0}\v Thanks for Becoming my Friend!\v I now have {1} Friends!", WF.user.displayName, VRCUser.CurrentUser.Friends.Count));

                ZuxiBioUpdate.SendUpdate();
            }



        }


        internal static void AcceptRequest(FriendRequest item)
        {

            if (JsonConfig.IgnoredFriendRequests.Contains(item.SenderUserId))
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
                JsonConfig.AddUserToIgnored(ThisUser.Id);
                Console.WriteLine("Skipping Accepting Friend Request from user {0} Because there account is still a visiter", item.SenderUsername);

                Console.ForegroundColor = ConsoleColor.Cyan;
            }

        }











        public static List<FriendRequest> DecodeJson(string json)
        {
            // Deserialize the JSON string into a List<FriendRequest> object
            return JsonConvert.DeserializeObject<List<FriendRequest>>(json);

        }

        public class WebsocketFriend
        {
            [JsonProperty("userId")]
            public string id;
            [JsonProperty("user")]
            public RequestUser user;

           public class RequestUser
            {
                public string displayName;
            }

            public static WebsocketFriend Create(string json)
            {
                return JsonConvert.DeserializeObject<WebsocketFriend>(json);
            }

        }





        public class FriendRequest
        {
            public string Id { get; set; }
            public string SenderUserId { get; set; }
            public string SenderUsername { get; set; }
            public string Type { get; set; }
            public string Message { get; set; }
            public object Details { get; set; }
            public bool Seen { get; set; }
            public DateTime CreatedAt { get; set; }
        }


    }
}
