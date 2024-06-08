using Zuxi.OSC.Modules.FriendRequest.Json;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Modules.FriendRequests
{
    public class FriendsMain
    {
        internal static WebsocketWrapper Websocket = null;
        internal static HClient HClient = null;
        public static Action<string> OnNewRequest;
        public static bool Initialize()
        {
            Console.WriteLine("AuthCookie: " + Config.AuthCookie);

            Console.WriteLine("Ignored FriendRequest Count: " + Config.IgnoredFriendRequests.Count);

            HClient = new HClient();
            if (HClient.CheckAuthStatus().Contains("Missing Credentials"))
            {
                throw new Exception("Failed Auth Check With VRChat Check Auth Cookie");
            }

            Websocket =  new WebsocketWrapper("wss://pipeline.vrchat.cloud/?authToken=" + Config.AuthCookie, FriendRequestHandler.OnWebsocketRequest);
            Websocket.Connect();
            string userinfojson = HClient.CrawlForUserInfo();

            VRCUser VRCU = VRCUser.CreateVRCUser(userinfojson);

            Console.Title = string.Format("Current User {0} | Friend Count {1}", VRCU.DisplayName, VRCU.Friends.Count);

            Console.WriteLine("Current Friends: {0}", VRCU.Friends.Count);

            ChatboxManager.AddNewMessageToChatboxQue(string.Format("Current Friends: {0}", VRCU.Friends.Count));

            FriendRequestHandler.FetchVRChatRequestsAndAcceptAll();

            return true;
        }







    }
}
