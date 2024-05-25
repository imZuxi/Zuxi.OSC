using System;
using System.IO;
using System.Net.NetworkInformation;
using Zuxi.OSC.Config;
using Zuxi.OSC.Module.FriendRequests;
using Zuxi.OSC.Modules.FriendRequests.OSC;

namespace Zuxi.OSC.Modules.FriendRequests
{
    public class FriendsMain
    {
        internal static HClient HClient = null;
        public static Action<string> OnNewRequest;
       public static bool Initialize(Action<string> OnNewFRequest)
        {



            OnNewRequest = OnNewFRequest;
           
           
            Console.WriteLine("AuthCookie: " + JsonConfig.AuthCookie);
            
            Console.WriteLine("Ignored FriendRequest Count: " + JsonConfig.IgnoredFriendRequests.Count);

            HClient = new HClient();
            if (HClient.CheckAuthStatus().Contains("Missing Credentials"))
            {
                throw new Exception("Failed Auth Check With VRChat Check Auth Cookie");
            }

            new Websocket("wss://pipeline.vrchat.cloud/?authToken=" + JsonConfig.AuthCookie);

            string userinfojson = HClient.CrawlForUserInfo();

            VRCUser VRCU = VRCUser.CreateVRCUser(userinfojson);

            Console.Title = string.Format("Current User {0} | Friend Count {1}", VRCU.DisplayName, VRCU.Friends.Count);

            Console.WriteLine("Current Friends: {0}", VRCU.Friends.Count);

            OnNewFRequest(string.Format("Current Friends: {0}", VRCU.Friends.Count));

            FriendRequests.FetchVRChatRequestsAndAcceptAll();

            return true;

         //   Console.ReadLine();
        }







    }
}
