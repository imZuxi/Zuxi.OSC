using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using static Zuxi.OSC.Modules.FriendRequests.FriendRequests;

namespace Zuxi.OSC.Modules.FriendRequests
{



    internal class Messages
    {
        public static FriendRequest DesteralizeMessageAsFriendRequest(string jsonString)
        {
            NotificationMessage notificationMessage = JsonConvert.DeserializeObject<NotificationMessage>(jsonString);

            // Check if the type is "notification" and deserialize the content
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

            // This property will hold the deserialized content
            [JsonIgnore]
            public FriendRequest Content { get; set; }

            // Method to deserialize the content based on the "type" field
            public void DeserializeContent()
            {
                if (Type == "notification")
                {
                    Content = FriendRequests.DecodeJson("["+ ContentJson + "]")[0];
                }
                else
                {
                    // Handle unsupported types or show an error message
                    Console.WriteLine("Unsupported type: " + Type);
                    Content = null;
                }
            }
        }

    }
}

