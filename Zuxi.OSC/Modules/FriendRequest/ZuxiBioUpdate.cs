using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zuxi.OSC.Modules.FriendRequest.Json;
using Zuxi.OSC.Modules.FriendRequests;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Module.FriendRequests
{
    internal class ZuxiBioUpdate
    {
      //  public static string BioTemplate = File.ReadAllText("ZuxiBio.txt");

        internal static void SendUpdate()
        {

            UserUpdate Update = new UserUpdate()
            {

                statusDescription = VRCUser.CurrentUser.StatusDescription,
                bioLinks = VRCUser.CurrentUser.BioLinks
            };

            Update.bio = Config.Bio.Replace("{CURRENTFRIENDCOUNT}", VRCUser.CurrentUser.Friends.Count.ToString());
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Update);
            string re = FriendsMain.HClient.MakeAPIPutRequest("users/" + VRCUser.CurrentUser.Id, json);
            if (re != "[]")
            {
                VRCUser.CreateVRCUser(re);
                Console.WriteLine("Bio Updated! ");
            }
        }
    }


    internal class UserUpdate
    {
        public string statusDescription { get; set; }
        public string bio { get; set; }

        public List<string> bioLinks { get; set; }
    }
}