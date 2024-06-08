using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuxi.OSC.Modules.FriendRequest.Json
{
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
}
