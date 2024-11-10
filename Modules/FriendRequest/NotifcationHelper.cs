// /*
//  *
//  * Zuxi.OSC - NotifcationHelper.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;
using Zuxi.OSC.Modules.FriendRequest.Json;

namespace Zuxi.OSC.Modules.FriendRequests;

internal class Messages
{
    public static friendRequest DesteralizeMessageAsFriendRequest(string jsonString)
    {
        var notificationMessage = JsonConvert.DeserializeObject<NotificationMessage>(jsonString);
        if (notificationMessage.Type == "notification")
        {
            notificationMessage.DeserializeContent();
            return notificationMessage.Content;
        }

        return null;
    }

    internal class NotificationMessage
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("content")] public string ContentJson { get; set; }

        [JsonIgnore] public friendRequest Content { get; set; }

        public void DeserializeContent()
        {
            if (Type == "notification")
            {
                Content = friendRequest.DecodeJson("[" + ContentJson + "]")[0];
            }
            else
            {
                Console.WriteLine("Unsupported type: " + Type);
                Content = null;
            }
        }
    }
}