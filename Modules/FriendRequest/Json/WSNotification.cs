using Newtonsoft.Json;

namespace Zuxi.OSC.Modules.FriendRequest.Json
{
    internal class WSNotification
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }


        public static WSNotification FromJson(string json)
        {
            return JsonConvert.DeserializeObject<WSNotification>(json);
        }
    }
    [Obsolete("Not used due to rewriting. However, it's still here since some other VRChat WebSocket data is sent can be used for this at which point I will remove the obsolete tag.")]
    public class WebsocketContainer
    {
        public class Content
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("type")]
            public string ContentType { get; set; }

            [JsonProperty("senderUserId")]
            public string SenderUserId { get; set; }

            [JsonProperty("senderUsername")]
            public string SenderUsername { get; set; }

            [JsonProperty("receiverUserId")]
            public string ReceiverUserId { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("details")]
            public object Details { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }
            public static Content FromJson(string json)
            {
                return JsonConvert.DeserializeObject<Content>(json);
            }
        }
    }
}

