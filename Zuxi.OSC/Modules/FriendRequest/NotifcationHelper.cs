using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Zuxi.OSC.Modules.FriendRequest.Json;

namespace Zuxi.OSC.Modules.FriendRequests
{



    internal class Messages
    {
        public static JFriendRequest DesteralizeMessageAsFriendRequest(string jsonString)
        {
            NotificationMessage notificationMessage = JsonConvert.DeserializeObject<NotificationMessage>(jsonString);
            if (notificationMessage.Type == "notification")
            {
                notificationMessage.DeserializeContent();
                return notificationMessage.Content;
            }
            return null;
        }

        internal class NotificationMessage
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("content")]
            public string ContentJson { get; set; }

            [JsonIgnore]
            public JFriendRequest Content { get; set; }
            public void DeserializeContent()
            {
                if (Type == "notification")
                {
                    Content = JFriendRequest.DecodeJson("["+ ContentJson + "]")[0];
                }
                else
                {
                   
                    Console.WriteLine("Unsupported type: " + Type);
                    Content = null;
                }
            }
        }

    }
}

